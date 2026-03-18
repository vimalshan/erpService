using System.Net;
using System.Text.Json;
using FluentValidation;
using TimeAttendance.Domain.Exceptions;

namespace TimeAttendance.API.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for request {Method} {Path}",
                context.Request.Method, context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, errors) = exception switch
        {
            ValidationException ve => (
                HttpStatusCode.BadRequest,
                "Validation failed.",
                ve.Errors.Select(e => e.ErrorMessage).ToArray()),
            AbsenteeismNotFoundException or AbsenteeismMisNotFoundException => (
                HttpStatusCode.NotFound, exception.Message, Array.Empty<string>()),
            DomainValidationException dve => (
                HttpStatusCode.UnprocessableEntity, dve.Message,
                dve.Errors.ToArray()),
            UnauthorizedAccessException => (
                HttpStatusCode.Forbidden, "Access denied.", Array.Empty<string>()),
            OperationCanceledException => (
                HttpStatusCode.RequestTimeout, "The request was cancelled.", Array.Empty<string>()),
            _ => (HttpStatusCode.InternalServerError,
                "An unexpected error occurred.", Array.Empty<string>())
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            message,
            errors,
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
    }
}

public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        => app.UseMiddleware<GlobalExceptionMiddleware>();
}
