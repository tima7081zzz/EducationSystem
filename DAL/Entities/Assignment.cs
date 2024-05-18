using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

//move to no sql db (maybe?)
public class Assignment
{
    [Key]
    public int Id { get; set; }
    public Course Course { get; set; }
    public int CourseId { get; set; }
    public int MaxGrade { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int CreatorTeacherId { get; set; }
    public DateTimeOffset Deadline { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}