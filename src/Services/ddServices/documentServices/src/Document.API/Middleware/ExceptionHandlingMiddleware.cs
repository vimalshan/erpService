using System.Net;
using System.Text.Json;
using Document.Domain.Exceptions;

namespace Document.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        => (_next, _logger) = (next, logger);

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
        var (statusCode, title, detail, errors) = exception switch
        {
            ValidationException ve => (HttpStatusCode.BadRequest, "Validation Error", ve.Message, ve.Errors),
            NotFoundException nfe => (HttpStatusCode.NotFound, "Not Found", nfe.Message, (IDictionary<string, string[]>?)null),
            DomainException de => (HttpStatusCode.BadRequest, "Domain Error", de.Message, (IDictionary<string, string[]>?)null),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized", "You are not authorized.", (IDictionary<string, string[]>?)null),
            _ => (HttpStatusCode.InternalServerError, "Server Error", "An unexpected error occurred.", (IDictionary<string, string[]>?)null)
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Unhandled exception");

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            title,
            status = (int)statusCode,
            detail,
            errors
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
