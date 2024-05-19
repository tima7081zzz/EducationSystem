using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(UserId), nameof(AssignmentId))]
public class StudentAssignment
{
    [Key] public int Id { get; set; }
    public User User { get; set; } = default!;
    public int UserId { get; set; }
    public Assignment Assignment { get; set; } = default!;
    public int AssignmentId { get; set; }
    public double? Grade { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public StudentCourseTaskStatus Status { get; set; } = StudentCourseTaskStatus.NotSubmitted;

    public ICollection<StudentAssignmentAttachment> StudentAssignmentAttachments { get; set; } = [];
}

public enum StudentCourseTaskStatus
{
    NotSubmitted = 0,
    SubmittedLate = 1,
    SubmittedInTime = 2,
}