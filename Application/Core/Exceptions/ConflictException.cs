using System.Net;

namespace Core.Exceptions;

public class ConflictException : HttpNotSuccessException
{
    public ConflictException(string? message = null) : base(HttpStatusCode.Conflict, message)
    {
    }
}