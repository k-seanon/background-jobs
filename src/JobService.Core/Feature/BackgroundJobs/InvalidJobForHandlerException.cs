namespace JobService.Core.Feature.BackgroundJobs;

public class InvalidJobForHandlerException : Exception
{
    public IJob Job { get; init; }
    public InvalidJobForHandlerException(object job)
    {
        Job = (IJob) job;
    }
}