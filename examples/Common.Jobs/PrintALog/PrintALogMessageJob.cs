using JobService.Core.Feature.BackgroundJobs;

namespace Common.Jobs.PrintALog;

public class PrintALogMessageJob : IJob
{
    public string ProcessorId { get; init; }
    public Guid InstanceId { get; init; } = Guid.NewGuid();
    public string JobName { get; init; } = nameof(PrintALogMessageJob);
    
    public string Message { get; init; }
}