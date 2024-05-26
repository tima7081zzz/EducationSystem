using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Constants;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(UserId), nameof(AssignmentId), Name = Indexes.StudentAssignmentUserIdAssignmentId, IsUnique = true)]
public class StudentAssignment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public User User { get; set; } = default!;
    public int UserId { get; set; }
    public Assignment Assignment { get; set; } = default!;
    public int AssignmentId { get; set; }
    public double? Grade { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public StudentCourseTaskStatus Status { get; set; } = StudentCourseTaskStatus.NotSubmitted;

    [StringLength(255)]
    public string? SubmissionComment { get; set; }
    
    [StringLength(255)]
    public string? GradingComment { get; set; }

    public ICollection<StudentAssignmentAttachment> StudentAssignmentAttachments { get; set; } = [];
}

public enum StudentCourseTaskStatus
{
    NotSubmitted = 0,
    SubmittedLate = 1,
    SubmittedInTime = 2,
    Graded = 3,
}