using System.Net;
using System.Text.Json;
using FluentValidation;
using ApprovalGroup.Domain.Exceptions;

namespace ApprovalGroup.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        var (statusCode, message) = exception switch
        {
            ApprovalGroupNotFoundException or
            ApprovalGroupMapNotFoundException or
            UserMapNotFoundException or
            PullMatrixNotFoundException => (HttpStatusCode.NotFound, exception.Message),

            ValidationException ve => (HttpStatusCode.BadRequest, string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))),

            DuplicateApprovalGroupException => (HttpStatusCode.Conflict, exception.Message),

            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized."),

            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new { status = (int)statusCode, message };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        => app.UseMiddleware<GlobalExceptionMiddleware>();
}
