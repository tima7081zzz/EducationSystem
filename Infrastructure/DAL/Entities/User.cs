using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(Email))]
public class User
{
    [Key]
    public int Id { get; set; }
    public required string Fullname { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<Assignment> Assignments { get; set; } = [];
    public ICollection<Course> Courses { get; set; } = [];
    public ICollection<StudentAssignment> StudentAssignments { get; set; } = [];
    public ICollection<StudentAssignmentAttachment> StudentAssignmentAttachments { get; set; } = [];
    public ICollection<StudentCourse> StudentCourses { get; set; } = [];
    public ICollection<TeacherCourse> TeacherCourses { get; set; } = [];
}