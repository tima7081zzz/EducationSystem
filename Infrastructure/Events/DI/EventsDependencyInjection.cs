using Microsoft.Extensions.DependencyInjection;

namespace Events.DI;

public static class EventsDependencyInjection
{
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.AddSingleton(_ => EventHandlerResolver.BindAllHandlers());
        
        services.AddTransient<IBackgroundEventHandlerRunner, BackgroundEventHandlerRunner>();
        services.AddTransient<IEventRaiser, EventRaiser>();
        
        return services;
    }
}