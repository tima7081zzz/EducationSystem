using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(PublicId))]
public class Course
{
    [Key]
    public int Id { get; set; }
    public required string PublicId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public User CreatorUser { get; set; } = default!;
    public int CreatorUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<TeacherCourse> TeacherCourses { get; set; } = [];
    public ICollection<StudentCourse> StudentCourses { get; set; } = [];
    public ICollection<Assignment> Assignments { get; set; } = [];
}