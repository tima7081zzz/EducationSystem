using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Course;

public static class CourseDependencyInjection
{
    public static IServiceCollection AddCourse(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}