using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class TeacherCourse
{
    [Key]
    public int Id { get; set; }
    public User TeacherUser { get; set; }
    public int TeacherUserId { get; set; }
    public Course Course { get; set; }
    public int CourseId { get; set; }
}