using System.Reflection;

namespace Events;

public interface IEventHandlerResolver
{
    IList<Type> ResolveBackgroundHandlers(Type eventType);
}

public class EventHandlerResolver : IEventHandlerResolver
{
    private readonly IDictionary<Type, IList<Type>> _backgroundBinds = new Dictionary<Type, IList<Type>>();

    private void Bind(Type eventType, Type eventHandlerType)
    {
        if (typeof(IBackgroundEventHandler).IsAssignableFrom(eventHandlerType))
        {
            var handlers = ResolveBackgroundHandlers(eventType);
            if (!handlers.Contains(eventHandlerType))
            {
                handlers.Add(eventHandlerType);
            }
            
            _backgroundBinds.Add(eventType, handlers);
        }
    }

    public IList<Type> ResolveBackgroundHandlers(Type eventType)
    {
        return _backgroundBinds.TryGetValue(eventType, out var bind) ? bind : new List<Type>();
    }

    public static IEventHandlerResolver BindAllHandlers()
    {
        var resolver = new EventHandlerResolver();

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            IEnumerable<Type> assemblyTypes;
            try
            {
                assemblyTypes = assembly.GetTypes().Where(x => x.IsInterface);
            }
            catch (ReflectionTypeLoadException)
            {
                //Some of assemblies references are not included to current project
                continue;
            }
            
            foreach (var handlerInterfaceType in assemblyTypes)
            {
                var eventBindAttributes = handlerInterfaceType.GetCustomAttributes<EventBindAttribute>();

                foreach (var eventBindAttribute in eventBindAttributes)
                {
                    if (!typeof(IEvent).IsAssignableFrom(eventBindAttribute.EventType))
                    {
                        continue;
                    }

                    var handlerTypes = assembly
                        .GetTypes()
                        .Where(x => x.IsClass)
                        .Where(x => handlerInterfaceType.IsAssignableFrom(x))
                        .ToList();

                    if (handlerTypes.Count > 1 || !typeof(IEventHandler).IsAssignableFrom(handlerTypes.First()))
                    {
                        throw new InvalidOperationException(
                            "must be only one handler class for each member interface and it must be inherited from IEventHandler");
                    }

                    resolver.Bind(eventBindAttribute.EventType, handlerInterfaceType);
                }
            }
        }

        return resolver;
    }
}

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
public class EventBindAttribute : Attribute
{
    public readonly Type EventType;

    public EventBindAttribute(Type eventType)
    {
        EventType = eventType;
    }
}