using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class UserNotificationSettings
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    
    public bool IsEnabled { get; set; }
    public bool NewAssignmentEnabled { get; set; }
    public bool DeadlineReminderEnabled { get; set; }
    public bool GradingAssignmentEnabled { get; set; }
    
    public bool AssignmentSubmittedEnabled { get; set; }
}