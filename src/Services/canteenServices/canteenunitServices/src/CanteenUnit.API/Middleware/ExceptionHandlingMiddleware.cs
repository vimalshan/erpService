using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace CanteenUnit.API.Middleware;

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
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (status, message, errors) = exception switch
        {
            ValidationException vex => (HttpStatusCode.BadRequest, "Validation failed",
                vex.Errors.Select(e => e.ErrorMessage).ToArray()),
            System.Collections.Generic.KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message, Array.Empty<string>()),
            InvalidOperationException => (HttpStatusCode.Conflict, exception.Message, Array.Empty<string>()),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized", Array.Empty<string>()),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", Array.Empty<string>())
        };

        context.Response.StatusCode = (int)status;
        var response = new { status = (int)status, message, errors };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
