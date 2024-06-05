using System.Reflection;
using Assignment.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Assignment;

public static class AssignmentDependencyInjection
{
    public static IServiceCollection AddAssignment(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.ConfigureJob();
        
        return services;
    }
    
    private static IServiceCollection ConfigureJob(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("EnqueueAssignmentDeadlineReminderTimerJob");

            q.AddJob<EnqueueAssignmentDeadlineReminderTimerJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("EnqueueAssignmentDeadlineReminderTimerJob-trigger")
                .WithCronSchedule("0 0/10 * * * ?"));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        
        return services;
    }
}