namespace Events.Events;

public class AssignmentAddedEvent : EventBase<AssignmentAddedEventArgs>
{
    public AssignmentAddedEvent(AssignmentAddedEventArgs args) : base(args)
    {
    }
}

public class AssignmentAddedEventArgs : EventArgs
{
    public AssignmentAddedEventArgs(int assignmentId)
    {
        AssignmentId = assignmentId;
    }

    public int AssignmentId { get; set; }
}