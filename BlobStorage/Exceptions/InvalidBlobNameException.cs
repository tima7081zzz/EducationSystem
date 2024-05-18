namespace BlobStorage.Exceptions;

public class InvalidBlobNameException : Exception
{
    public InvalidBlobNameException(string? message) : base(message)
    {
    }
}