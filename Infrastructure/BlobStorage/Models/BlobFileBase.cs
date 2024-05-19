namespace BlobStorage;

public class BlobFileBase
{
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public required Stream InputStream { get; set; }
}