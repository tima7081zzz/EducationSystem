using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class StudentCourse
{
    [Key]
    public int Id { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public Course Course { get; set; }
    public int CourseId { get; set; }
}