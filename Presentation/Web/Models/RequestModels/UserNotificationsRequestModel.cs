namespace Web.Models.RequestModels;

public class UserNotificationsRequestModel
{
    public bool IsEnabled { get; set; }
    public bool NewAssignmentEnabled { get; set; }
    public bool DeadlineReminderEnabled { get; set; }
    public bool GradingAssignmentEnabled { get; set; }
    public bool AssignmentSubmittedEnabled { get; set; }
}