using System.Reflection;
using Events;
using Microsoft.Extensions.DependencyInjection;

namespace Handlers.DI;

public static class HandlersDependencyInjection
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.Scan(x => x
                .FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(c => c.AssignableTo<IEventHandler>())
                .AsMatchingInterface());
        
        return services;
    }
}