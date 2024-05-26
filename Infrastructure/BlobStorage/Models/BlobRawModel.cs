namespace BlobStorage.Models;

public class BlobRawModel
{
    public byte[] BinaryData { get; set; } = [];
    public required string ContentType { get; set; }
}