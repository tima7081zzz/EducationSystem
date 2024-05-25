using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Constants;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(UserId), nameof(CourseId), Name = Indexes.StudentCourseUserIdCourseId, IsUnique = true)]
public class StudentCourse
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public User User { get; set; } = default!;
    public int UserId { get; set; }
    public Course Course { get; set; } = default!;
    public int CourseId { get; set; }
}