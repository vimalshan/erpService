using System.Net;
using System.Text.Json;
using FluentValidation;
using FilingAndArchiveService.Domain.Exceptions;

namespace FilingAndArchiveService.API.Middleware;

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
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var (statusCode, title, errors) = exception switch
        {
            ValidationException ve => (
                HttpStatusCode.BadRequest,
                "Validation failed",
                ve.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToArray()),

            Domain.Exceptions.FileNotFoundException nfe => (
                HttpStatusCode.NotFound,
                "Resource not found",
                new[] { nfe.Message }),

            FilingDomainException de => (
                HttpStatusCode.UnprocessableEntity,
                "Domain error",
                new[] { de.Message }),

            UnauthorizedAccessException uae => (
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                new[] { uae.Message }),

            _ => (
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred",
                new[] { "Please try again later." })
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        var response = new
        {
            type = $"https://httpstatuses.io/{(int)statusCode}",
            title,
            status = (int)statusCode,
            errors,
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
