using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Constants;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(UserId), Name = Indexes.UserNotificationSettingsUserId, IsUnique = true)]
public class UserNotificationSettings
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public bool IsEnabled { get; set; } = true;
    public bool NewAssignmentEnabled { get; set; } = true;
    public bool DeadlineReminderEnabled { get; set; } = true;
    public bool GradingAssignmentEnabled { get; set; } = true;
    public bool AssignmentSubmittedEnabled { get; set; } = true;
}