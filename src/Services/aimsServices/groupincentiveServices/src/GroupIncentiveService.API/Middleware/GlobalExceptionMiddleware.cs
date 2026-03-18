using GroupIncentiveService.Domain.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace GroupIncentiveService.API.Middleware;

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
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message, errors) = exception switch
        {
            NotFoundException nfe => (HttpStatusCode.NotFound, nfe.Message, (object?)null),
            BusinessRuleViolationException br => (HttpStatusCode.UnprocessableEntity, br.Message, (object?)null),
            DomainException de => (HttpStatusCode.BadRequest, de.Message, (object?)null),
            ValidationException ve => (HttpStatusCode.BadRequest, "Validation failed.",
                ve.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized.", (object?)null),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", (object?)null)
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            StatusCode = (int)statusCode,
            Message = message,
            Errors = errors
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
