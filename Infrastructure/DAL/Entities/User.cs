using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Constants;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(Email), IsUnique = true, Name = Indexes.UserEmail)]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [StringLength(255)]
    public required string Fullname { get; set; }
    
    [StringLength(255)]
    public required string Password { get; set; }
    
    [StringLength(255)]
    public required string Email { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<Assignment> Assignments { get; set; } = [];
    public ICollection<Course> Courses { get; set; } = [];
    public ICollection<StudentAssignment> StudentAssignments { get; set; } = [];
    public ICollection<StudentAssignmentAttachment> StudentAssignmentAttachments { get; set; } = [];
    public ICollection<StudentCourse> StudentCourses { get; set; } = [];
    public ICollection<TeacherCourse> TeacherCourses { get; set; } = [];
    public ICollection<RecommendedCourse> RecommendedCourses { get; set; } = [];

    public UserNotificationSettings NotificationSettings { get; set; }
}