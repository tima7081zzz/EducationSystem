using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class StudentAssignmentAttachment
{
    [Key] public int Id { get; set; }
    public int StudentAssignmentId { get; set; }
    public StudentAssignment StudentAssignment { get; set; } = default!;
    public User StudentUser { get; set; } = default!;
    public int StudentUserId { get; set; }
    public required string BlobName { get; set; }
}