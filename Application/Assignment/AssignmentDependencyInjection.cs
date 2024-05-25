using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment;

public static class AssignmentDependencyInjection
{
    public static IServiceCollection AddAssignment(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}