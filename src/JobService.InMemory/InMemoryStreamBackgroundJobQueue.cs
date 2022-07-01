using JobService.Core.Feature.BackgroundJobs;
using System.Threading.Channels;

namespace JobService.InMemory;

public class InMemoryStreamBackgroundJobQueue : IBackgroundJobQueue
{
    private readonly Channel<IJob> _queue;

    public InMemoryStreamBackgroundJobQueue()
    {
        var options = new BoundedChannelOptions(10)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<IJob>(options);
    }
    public async ValueTask<IJob> DequeueAsync(CancellationToken cancellationToken)
    {
        var task = await _queue.Reader.ReadAsync(cancellationToken);
        return task;
    }
    public async ValueTask QueueAsync(IJob job, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(job, nameof(job));
        await _queue.Writer.WriteAsync(job, cancellationToken);
    }
}