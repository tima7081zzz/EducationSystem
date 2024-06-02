using System.Net;

namespace Core.Exceptions;

public abstract class HttpNotSuccessException : Exception
{
    protected HttpNotSuccessException(HttpStatusCode statusCode, string? message)
    {
        StatusCode = statusCode;

        if (message is not null)
        {
            base.Data["Message"] = message;
        }
    }

    public HttpStatusCode StatusCode { get; }
}