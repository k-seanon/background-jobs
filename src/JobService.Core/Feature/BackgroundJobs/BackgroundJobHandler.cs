namespace JobService.Core.Feature.BackgroundJobs;

public abstract class BackgroundJobHandler<TJob> : IJobHandler<TJob>
    where TJob : class,IJob
{
    public abstract ValueTask Run(TJob job, CancellationToken cancellationToken = default);

    public ValueTask Run(object job, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(nameof(job));
        var typedJob = job as TJob;
        if (typedJob is null)
            throw new InvalidJobForHandlerException(job);
        
        return Run(typedJob, cancellationToken);
    }
}