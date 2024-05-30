namespace Events;

public static class EventRaiserExtensions
{
    public static async ValueTask Raise(this IEventRaiser eventRaiser, IEvent @event, CancellationToken ct)
    {
        await eventRaiser.RaiseBatch(new[] {@event}, ct);
    }
}