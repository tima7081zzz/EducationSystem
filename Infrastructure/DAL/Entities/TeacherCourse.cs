using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Constants;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(UserId), Name = Indexes.TeacherCourseTeacherUserId)]
public class TeacherCourse
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public User User { get; set; } = default!;
    public int UserId { get; set; }
    public Course Course { get; set; } = default!;
    public int CourseId { get; set; }
    public bool IsNotificationsEnabled = true;
}