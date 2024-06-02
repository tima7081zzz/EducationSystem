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

    public async Task InvokeAsync(HttpContext context, ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (HttpNotSuccessException e)
        {
            var statusCode = e.StatusCode;

            context.Response.StatusCode = (int) statusCode;
            context.Response.ContentType = "application/json";

            if (e.Data.Count > 0)
            {
                await context.Response.WriteAsJsonAsync(e.Data, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }

            logger.LogInformation(exception: e, message: "HTTP call is not success. Status {statusCode}", statusCode);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsJsonAsync(e.Message, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            logger.LogError(exception: e, message: "HTTP Internal Server Error");
        }
    }
}