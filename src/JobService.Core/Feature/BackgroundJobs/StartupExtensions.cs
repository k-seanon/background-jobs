using Microsoft.Extensions.DependencyInjection;

namespace JobService.Core.Feature.BackgroundJobs;

public static class StartupExtensions
{
    public static IServiceCollection AddBackgroundJobs<TBackgroundJobQueue>(this IServiceCollection services)
        where TBackgroundJobQueue : class, IBackgroundJobQueue
    {
        services.AddSingleton<IBackgroundJobQueue, TBackgroundJobQueue>();
        services.AddHostedService<BackgroundJobHostedService>();
        return services;
    }

    public static IServiceCollection AddBackgroundJobHandler<TJob, TJobHandler>(this IServiceCollection services)
        where TJob : class, IJob
        where TJobHandler : class, IJobHandler<TJob>
    {
        services.AddTransient<IJobHandler<TJob>, TJobHandler>();

        return services;
    }
}