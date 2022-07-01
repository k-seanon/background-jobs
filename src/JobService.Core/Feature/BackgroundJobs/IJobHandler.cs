namespace JobService.Core.Feature.BackgroundJobs;

public interface IJobHandler
{
    ValueTask Run(object job, CancellationToken cancellationToken);
}

public interface IJobHandler<TJob> : IJobHandler
    where TJob : class, IJob 
{
    new ValueTask Run(TJob job, CancellationToken cancellationToken);
}