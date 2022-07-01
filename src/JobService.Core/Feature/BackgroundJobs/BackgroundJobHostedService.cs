using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace JobService.Core.Feature.BackgroundJobs;

public class BackgroundJobHostedService : BackgroundService
{
    private readonly ILogger<BackgroundJobHostedService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IBackgroundJobQueue _jobQueue;

    public BackgroundJobHostedService(ILogger<BackgroundJobHostedService> logger, IServiceScopeFactory scopeFactory, IBackgroundJobQueue jobQueue)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _jobQueue = jobQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{Service} is starting", nameof(BackgroundJobHostedService));
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var job = await _jobQueue.DequeueAsync(stoppingToken);
                _logger.LogInformation("{@Job} has been picked up from queue", job);
                using var scope = _scopeFactory.CreateScope();
                var handlers = GetHandlers(scope.ServiceProvider, job);
                await Parallel.ForEachAsync(handlers, stoppingToken, HandleJob(job));
            }
            catch (OperationCanceledException e)
            {
                _logger.LogInformation(exception: e, "Cancel called on stopping token, service is shutting down");
            }
            catch (Exception e)
            {
                //log and continue, throwing here will terminate the hosted service and no jobs will be processed
                _logger.LogError(exception: e, message: "Error in the background job hosted service");
            }
        }

        //processing of job used in the loop above
        Func<IJobHandler, CancellationToken, ValueTask> HandleJob(IJob job) => async (handler, token) =>
        {
            var timer = Stopwatch.StartNew();
            try
            {
                _logger.LogInformation("{Handler} is processing {@Job}", handler.GetType().Name, job);
                await handler.Run(job, token);
                timer.Stop();
                _logger.LogInformation("{@Job} completed successfully {Elapsed}ms", job, timer.ElapsedMilliseconds.ToString());
            }
            catch (Exception e)
            {
                //will be extended to leverage policy based retry models
                _logger.LogError(exception: e, message: "Error processing {@Job} with {Handler}", job, handler);
            }
        };
    }


    /// <summary>
    /// retrieves all handlers for the provided job from the provided service provider 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="job"></param>
    /// <returns></returns>
    private static IEnumerable<IJobHandler> GetHandlers(IServiceProvider serviceProvider, IJob job)
    {
        var type = typeof(IJobHandler<>);
        var qualified = type.MakeGenericType(job.GetType());

        foreach (var handler in serviceProvider.GetServices(qualified))
        {
            yield return handler as IJobHandler;
        }
    }
}