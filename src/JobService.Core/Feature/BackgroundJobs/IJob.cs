namespace JobService.Core.Feature.BackgroundJobs;

public interface IJob
{
    public string ProcessorId { get; init; }
    public Guid InstanceId { get; init; }
    public string JobName { get; init; }
}