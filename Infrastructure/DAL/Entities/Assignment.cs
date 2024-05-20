using System.ComponentModel.DataAnnotations;
using DAL.Constants;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

//move to no sql db (maybe?)
[Index(nameof(CourseId), Name = Indexes.AssignmentCourseId)]
public class Assignment
{
    [Key]
    public int Id { get; set; }
    public Course Course { get; set; } = default!;
    public int CourseId { get; set; }
    public int? MaxGrade { get; set; }
    
    [StringLength(255)]
    public required string Title { get; set; }
    
    [StringLength(512)]
    public string? Description { get; set; }
    public User CreatorTeacher { get; set; } = default!;
    public int CreatorTeacherId { get; set; }
    public DateTimeOffset Deadline { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}