using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class StudentAssignmentAttachment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int StudentAssignmentId { get; set; }
    public StudentAssignment StudentAssignment { get; set; } = default!;
    public User StudentUser { get; set; } = default!;
    public int StudentUserId { get; set; }
    
    [StringLength(255)]
    public required string BlobName { get; set; }
    
    [StringLength(512)]
    public required string FileName { get; set; }
}