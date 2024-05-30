namespace Events;

public interface IEvent
{
    EventArgs GetArgs();
}

public interface IBackgroundEventHandler : IEventHandler;