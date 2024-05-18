using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class StudentAssignment
{
    [Key]
    public int Id { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public Assignment Assignment { get; set; }
    public int AssignmentId { get; set; }
    public double? Grage { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public StudentCourseTaskStatus Status { get; set; } = StudentCourseTaskStatus.NotSubmitted;
}

public enum StudentCourseTaskStatus
{
    NotSubmitted = 0,
    SubmittedLate = 1,
    SubmittedInTime = 2,
}