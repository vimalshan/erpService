using System.Net;
using System.Text.Json;
using FluentValidation;

namespace WarehouseStructure.API.Middleware;

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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                JsonSerializer.Serialize(new
                {
                    error = "Validation failed",
                    details = validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                })
            ),
            System.Collections.Generic.KeyNotFoundException => (
                HttpStatusCode.NotFound,
                JsonSerializer.Serialize(new { error = exception.Message })
            ),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                JsonSerializer.Serialize(new { error = "Unauthorized" })
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                JsonSerializer.Serialize(new { error = "An internal server error occurred." })
            )
        };

        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(message);
    }
}
