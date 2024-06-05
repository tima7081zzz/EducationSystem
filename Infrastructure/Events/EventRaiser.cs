namespace Events;

public interface IEventRaiser
{
    ValueTask RaiseBatch(IEnumerable<IEvent> events, CancellationToken ct);
}

public class EventRaiser : IEventRaiser
{
    private readonly IEventHandlerResolver _eventHandlerResolver;
    private readonly IBackgroundEventHandlerRunner _backgroundEventHandlerRunner;

    public EventRaiser(IEventHandlerResolver eventHandlerResolver, IBackgroundEventHandlerRunner backgroundEventHandlerRunner)
    {
        _eventHandlerResolver = eventHandlerResolver;
        _backgroundEventHandlerRunner = backgroundEventHandlerRunner;
    }

    public ValueTask RaiseBatch(IEnumerable<IEvent> events, CancellationToken ct)
    {
        var eventList = new List<IEvent>(events);

        foreach (var @event in eventList)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (@event is null)
            {
                continue;
            }

            var eventArgs = @event.GetArgs();
            var handlerTypes = _eventHandlerResolver.ResolveBackgroundHandlers(@event.GetType());

            foreach (var handlerType in handlerTypes)
            {
                _backgroundEventHandlerRunner.RunEventHandler(handlerType, eventArgs);
            }
        }

        return ValueTask.CompletedTask;
    }
}