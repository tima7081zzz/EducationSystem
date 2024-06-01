namespace Events.Events;

public class AssignmentDeadlineApproachingEvent : EventBase<AssignmentDeadlineApproachingEventArgs>
{
    public AssignmentDeadlineApproachingEvent(AssignmentDeadlineApproachingEventArgs args) : base(args)
    {
    }
}

public class AssignmentDeadlineApproachingEventArgs : EventArgs
{
    public AssignmentDeadlineApproachingEventArgs(int assignmentId)
    {
        AssignmentId = assignmentId;
    }

    public int AssignmentId { get; set; }
}