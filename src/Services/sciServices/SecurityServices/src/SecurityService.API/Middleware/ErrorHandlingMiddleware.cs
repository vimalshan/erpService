using System.Net;
using System.Text.Json;
using FluentValidation;
using SecurityService.Domain.Exceptions;

namespace SecurityService.API.Middleware;

public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, errors) = exception switch
        {
            ValidationException ve => (HttpStatusCode.BadRequest, "Validation failed.",
                ve.Errors.Select(e => e.ErrorMessage).ToArray()),
            UserNotFoundException nfe => (HttpStatusCode.NotFound, nfe.Message, Array.Empty<string>()),
            RoleNotFoundException rfe => (HttpStatusCode.NotFound, rfe.Message, Array.Empty<string>()),
            DomainException de => (HttpStatusCode.UnprocessableEntity, de.Message, Array.Empty<string>()),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized.", Array.Empty<string>()),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", Array.Empty<string>())
        };

        var payload = JsonSerializer.Serialize(new
        {
            status = (int)statusCode,
            message,
            errors
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(payload);
    }
}
