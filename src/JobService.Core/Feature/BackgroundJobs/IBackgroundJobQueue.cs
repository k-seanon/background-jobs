namespace JobService.Core.Feature.BackgroundJobs;

public interface IBackgroundJobQueue
{
    ValueTask<IJob> DequeueAsync(CancellationToken cancellationToken = default);
    ValueTask QueueAsync(IJob job, CancellationToken cancellationToken = default);
}