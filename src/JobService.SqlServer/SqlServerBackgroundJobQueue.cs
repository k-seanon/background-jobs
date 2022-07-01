using JobService.Core.Feature.BackgroundJobs;
using JobService.SqlServer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace JobService.SqlServer;

public class SqlServiceBackgroundJobQueueOptions
{
    public static string ConfigurationKey => "BackgroundQueue";
    public string ConnectionString { get; init; }
}

public class SqlServerBackgroundJobQueue<TDbContext> : IBackgroundJobQueue
    where TDbContext : JobsDbContext
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SqlServerBackgroundJobQueue(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    public async ValueTask<IJob> DequeueAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TDbContext>();

        var sql = @"UPDATE Top (1) Jobs SET JobStatus = 'Queued'
                    OUTPUT inserted.Id
                    WHERE JobStatus NOT IN ('Failed', 'Success', 'Processing')";
        //TODO introduce date sorting

        await using var connection = db.Database.GetDbConnection() as SqlConnection; //fragile for time
        await using var command = new SqlCommand(sql, connection);

        await connection!.OpenAsync(cancellationToken); //TODO remove this when fragility addressed

        command.Parameters.AddWithValue("status", JobStatus.Processing.ToString());
        
        var id = (long)await command.ExecuteScalarAsync(cancellationToken);

        var entity = await db.Jobs.FirstOrDefaultAsync(e => e.Id == id);
        return (IJob)entity.JobDto;
    }

    public async ValueTask QueueAsync(IJob job, CancellationToken cancellationToken = default)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TDbContext>();
        await db.Jobs.AddAsync(JobEntity.Create(job), cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }
}