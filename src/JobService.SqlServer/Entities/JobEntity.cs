using JobService.Core.Feature.BackgroundJobs;

namespace JobService.SqlServer.Entities;
public enum JobStatus
{
    Failed,
    Error,
    Queued,
    Processing,
    Success,
}
public class JobEntity
{
    public long Id { get; set; }
    public Guid JobInstanceId { get; set; }
    public string JobType { get; set; }
    public object JobDto { get; set; } //TODO figure this out
    
    public JobStatus JobStatus { get; set; }

    public static JobEntity Create(IJob job) => new()
    {
        JobInstanceId = Guid.NewGuid(),
        JobDto = job,
        JobType = job.GetType().Name,
        JobStatus =  JobStatus.Queued,
    };
}