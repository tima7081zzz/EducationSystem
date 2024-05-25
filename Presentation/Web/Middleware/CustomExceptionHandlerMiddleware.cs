using System.Net;
using System.Text.Json;
using Core.Exceptions;

namespace Web.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception e)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        code = e switch
        {
            EntityNotFoundException => HttpStatusCode.NotFound,
            _ => code
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) code;

        if (result == string.Empty)
        {
            result = JsonSerializer.Serialize(e.Message);
        }

        context.Response.StatusCode = (int) code;
        await context.Response.WriteAsync(result);
    }
}