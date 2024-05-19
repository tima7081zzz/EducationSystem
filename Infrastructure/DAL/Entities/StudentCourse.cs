using System.ComponentModel.DataAnnotations;
using DAL.Constants;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(UserId), nameof(CourseId), Name = Indexes.StudentCourseUserIdCourseId, IsUnique = true)]
public class StudentCourse
{
    [Key]
    public int Id { get; set; }
    public User User { get; set; } = default!;
    public int UserId { get; set; }
    public Course Course { get; set; } = default!;
    public int CourseId { get; set; }
}