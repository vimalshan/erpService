using EmployeeManagement.Domain.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace EmployeeManagement.API.Middleware;

/// <summary>Global exception handler middleware converting domain exceptions to HTTP responses.</summary>
public sealed class ExceptionHandlingMiddleware
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
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, errors) = exception switch
        {
            EmployeeNotFoundException ex => (HttpStatusCode.NotFound, ex.Message, Array.Empty<string>()),
            DuplicateEmployeeException ex => (HttpStatusCode.Conflict, ex.Message, Array.Empty<string>()),
            ProbationAlreadyCompletedException ex => (HttpStatusCode.UnprocessableEntity, ex.Message, Array.Empty<string>()),
            ValidationException ex => (HttpStatusCode.BadRequest, "One or more validation errors occurred.",
                ex.Errors.Select(e => e.ErrorMessage).ToArray()),
            UnauthorizedAccessException => (HttpStatusCode.Forbidden, "Access denied.", Array.Empty<string>()),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", Array.Empty<string>())
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            message,
            errors
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
