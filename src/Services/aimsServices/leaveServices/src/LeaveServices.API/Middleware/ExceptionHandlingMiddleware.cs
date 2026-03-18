using System.Net;
using System.Text.Json;
using FluentValidation;

namespace LeaveServices.API.Middleware;

/// <summary>
/// Global exception handler that converts domain / validation exceptions to well-formed RFC 7807 responses.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation failure: {Errors}", ex.Message);
            context.Response.StatusCode  = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/problem+json";
            var problem = new
            {
                type    = "https://tools.ietf.org/html/rfc7807",
                title   = "Validation Error",
                status  = 400,
                errors  = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
        catch (System.Collections.Generic.KeyNotFoundException ex)
        {
            _logger.LogWarning("Not found: {Message}", ex.Message);
            context.Response.StatusCode  = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/problem+json";
            var problem = new { title = "Not Found", status = 404, detail = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Business rule violation: {Message}", ex.Message);
            context.Response.StatusCode  = (int)HttpStatusCode.UnprocessableEntity;
            context.Response.ContentType = "application/problem+json";
            var problem = new { title = "Business Rule Violation", status = 422, detail = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode  = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/problem+json";
            var problem = new { title = "Internal Server Error", status = 500, detail = "An unexpected error occurred." };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
