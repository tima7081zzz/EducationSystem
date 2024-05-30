namespace Events;

public class EventBase<TEventArgs> : IEvent where TEventArgs : EventArgs
{
    public EventArgs Args { get; }

    protected EventBase(TEventArgs args)
    {
        Args = args;
    }

    public EventArgs GetArgs()
    {
        return Args;
    }
}