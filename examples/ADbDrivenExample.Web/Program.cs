using ADbDrivenExample.Web;
using Common.Jobs.PrintALog;
using JobService.Core.Feature.BackgroundJobs;
using JobService.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddBackgroundJobs<SqlServerBackgroundJobQueue<JobsDbContext>>()
    .AddBackgroundJobHandler<PrintALogMessageJob, PrintALogMessageJobHandler>();

builder.Services.AddDbContext<JobsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("default"),
        b =>
        {
            b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            b.MigrationsAssembly("JobService.SqlServer");
        });
                
});

var app = builder.Build();

app.UseHttpsRedirection();

var running = app.RunAsync();
await app.Services.ApplyMigration<JobsDbContext>();

var jobQueue = app.Services.GetService<IBackgroundJobQueue>();
jobQueue?.QueueAsync(new PrintALogMessageJob()
{
    ProcessorId = IAm.MyId.ToString(),
    Message = "I'm a simple thing",
});

await running;


public static class StartupHelpers
{
    public static async Task ApplyMigration<TDbContext>(this IServiceProvider serviceProvider)
        where TDbContext : DbContext
    {
        using var scope = serviceProvider.CreateScope(); 
        var db = scope.ServiceProvider.GetRequiredService<TDbContext>();
        var migrations = (await db.Database.GetPendingMigrationsAsync()).ToArray();
        if (migrations.Any())
        {
            //Log.Information("Applying Migrations {@Migrations}", migrations);
            await db.Database.MigrateAsync();
        }
    }
}