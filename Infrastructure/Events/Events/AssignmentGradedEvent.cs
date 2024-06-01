namespace Events.Events;

public class AssignmentGradedEvent : EventBase<AssignmentGradedEventArgs>
{
    public AssignmentGradedEvent(AssignmentGradedEventArgs args) : base(args)
    {
    }
}

public class AssignmentGradedEventArgs : EventArgs
{
    public AssignmentGradedEventArgs(int studentAssignmentId)
    {
        StudentAssignmentId = studentAssignmentId;
    }

    public int StudentAssignmentId { get; set; }
}