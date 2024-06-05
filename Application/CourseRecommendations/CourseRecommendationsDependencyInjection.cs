using System.Reflection;
using CourseRecommendations.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CourseRecommendations;

public static class CourseRecommendationsDependencyInjection
{
    public static IServiceCollection AddCourseRecommendations(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.ConfigureJob();
        
        return services;
    }
    
    private static IServiceCollection ConfigureJob(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("EnqueueCourseRecommendationsTimerJob");

            q.AddJob<EnqueueCourseRecommendationsTimerJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("EnqueueCourseRecommendationsTimerJob-trigger")
                .WithCronSchedule("0 5/10 * * * ?"));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        
        return services;
    }
}