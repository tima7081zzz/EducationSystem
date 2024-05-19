using System.ComponentModel.DataAnnotations;
using DAL.Constants;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(TeacherUserId), Name = Indexes.TeacherCourseTeacherUserId)]
public class TeacherCourse
{
    [Key]
    public int Id { get; set; }
    public User TeacherUser { get; set; } = default!;
    public int TeacherUserId { get; set; }
    public Course Course { get; set; } = default!;
    public int CourseId { get; set; }
}