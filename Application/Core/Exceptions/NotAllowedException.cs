using System.Net;

namespace Core.Exceptions;

public class NotAllowedException : HttpNotSuccessException
{
    public NotAllowedException(string? message = null) : base(HttpStatusCode.Forbidden, message)
    {
    }
    
    public static void ThrowIf(bool condition, string? message = null)
    {
        if (condition)
        {
            throw new NotAllowedException(message);
        }
    }
    
    public static void ThrowIfNull(object? obj, string? message = null)
    {
        if (obj is null)
        {
            throw new NotAllowedException(message);
        }
    }
}