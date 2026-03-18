using System.Net;
using System.Text.Json;
using FluentValidation;

namespace TrustService.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                new ErrorResponse("Validation Error",
                    validationEx.Errors.Select(e => e.ErrorMessage).ToArray())),

            System.Collections.Generic.KeyNotFoundException => (
                HttpStatusCode.NotFound,
                new ErrorResponse("Not Found", new[] { exception.Message })),

            InvalidOperationException => (
                HttpStatusCode.Conflict,
                new ErrorResponse("Conflict", new[] { exception.Message })),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                new ErrorResponse("Unauthorized", new[] { "Access denied." })),

            _ => (
                HttpStatusCode.InternalServerError,
                new ErrorResponse("Internal Server Error", new[] { "An unexpected error occurred." }))
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception occurred");
        }
        else
        {
            _logger.LogWarning(exception, "Handled exception: {StatusCode}", statusCode);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(message, options));
    }
}

public record ErrorResponse(string Title, string[] Errors);
