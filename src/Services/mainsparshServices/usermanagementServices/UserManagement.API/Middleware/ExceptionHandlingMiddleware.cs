using System.Net;
using System.Text.Json;
using UserManagement.Application.Common.Exceptions;
using UserManagement.Domain.Common;

namespace UserManagement.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, title, errors) = exception switch
        {
            ValidationException ve => (HttpStatusCode.BadRequest, "Validation Failed", (object)ve.Errors),
            NotFoundException nfe => (HttpStatusCode.NotFound, "Not Found", (object)new { detail = nfe.Message }),
            DomainException de => (HttpStatusCode.UnprocessableEntity, "Domain Error", (object)new { detail = de.Message }),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized", (object)new { detail = "Access denied." }),
            _ => (HttpStatusCode.InternalServerError, "Internal Server Error", (object)new { detail = "An unexpected error occurred." })
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            title,
            errors,
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
