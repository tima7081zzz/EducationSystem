using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class Course
{
    [Key]
    public int Id { get; set; }
    public required string PublicId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public User CreatorUser { get; set; }
    public int CreatorUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public IEnumerable<TeacherCourse> TeacherCourses { get; set; } = [];
    public IEnumerable<StudentCourse> StudentCourses { get; set; } = [];
}