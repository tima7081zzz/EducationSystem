using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(TeacherUserId))]
public class TeacherCourse
{
    [Key]
    public int Id { get; set; }
    public User TeacherUser { get; set; } = default!;
    public int TeacherUserId { get; set; }
    public Course Course { get; set; } = default!;
    public int CourseId { get; set; }
}