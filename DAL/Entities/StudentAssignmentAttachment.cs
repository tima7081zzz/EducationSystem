using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class StudentAssignmentAttachment
{
    [Key] public int Id { get; set; }
    public StudentAssignment StudentAssignment { get; set; }
    public int StudentAssignmentId { get; set; }
    public User StudentUser { get; set; }
    public int StudentUserId { get; set; }
    //public int? Type { get; set; }
    public required string BlobName { get; set; }
}