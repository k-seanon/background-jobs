using JobService.Core.Feature.BackgroundJobs;
using Microsoft.Extensions.Logging;

namespace Common.Jobs.PrintALog;

public class PrintALogMessageJobHandler : BackgroundJobHandler<PrintALogMessageJob>
{
    private readonly ILogger<PrintALogMessageJobHandler> _logger;

    public PrintALogMessageJobHandler(ILogger<PrintALogMessageJobHandler> logger)
    {
        _logger = logger;
    }
    public override ValueTask Run(PrintALogMessageJob job, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("{Message}", job.Message);
        return ValueTask.CompletedTask;
    }
}