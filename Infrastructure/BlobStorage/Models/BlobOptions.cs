namespace BlobStorage.Models;

public class BlobOptions
{
    public const string SectionName = "Blob";
    public required string BlobConnectionString { get; set; }
    public required string StudentAssignmentAttachmentsContainerName { get; set; }
}