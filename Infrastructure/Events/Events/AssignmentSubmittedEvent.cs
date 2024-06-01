namespace Events.Events;

public class AssignmentSubmittedEvent : EventBase<AssignmentSubmittedEventArgs>
{
    public AssignmentSubmittedEvent(AssignmentSubmittedEventArgs args) : base(args)
    {
    }
}

public class AssignmentSubmittedEventArgs : EventArgs
{
    public AssignmentSubmittedEventArgs(int studentAssignmentId)
    {
        StudentAssignmentId = studentAssignmentId;
    }

    public int StudentAssignmentId { get; set; }
}