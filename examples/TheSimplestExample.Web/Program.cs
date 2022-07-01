using Common.Jobs.PrintALog;
using JobService.Core.Feature.BackgroundJobs;
using JobService.InMemory;
using TheSimplestExample.Web;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<BackgroundJobHostedService>();
builder.Services.AddSingleton<IBackgroundJobQueue, InMemoryStreamBackgroundJobQueue>();
builder.Services.AddTransient<IJobHandler<PrintALogMessageJob>, PrintALogMessageJobHandler>(); //manual registration, could easily use an assembly scan

var app = builder.Build();

var running =  app.RunAsync();
//add a job, check your logs
var queue = app.Services.GetService<IBackgroundJobQueue>();
await queue.QueueAsync(new PrintALogMessageJob()
{
    ProcessorId = IAm.MyId.ToString(),
    Message = "Hello from the job engine!"
});

await running;