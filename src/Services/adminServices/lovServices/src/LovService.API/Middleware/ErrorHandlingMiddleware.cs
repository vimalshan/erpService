using System.Net;
using System.Text.Json;
using FluentValidation;
using LovService.Domain.Exceptions;

namespace LovService.API.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (LovNotFoundException ex)
        {
            logger.LogWarning(ex, "Resource not found: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.NotFound, "NOT_FOUND", ex.Message);
        }
        catch (LovDomainException ex)
        {
            logger.LogWarning(ex, "Domain error: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.BadRequest, "DOMAIN_ERROR", ex.Message);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error");
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            await WriteErrorResponse(context, HttpStatusCode.BadRequest, "VALIDATION_ERROR",
                "One or more validation errors occurred.", errors);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Unauthorized: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.Unauthorized, "UNAUTHORIZED", ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteErrorResponse(context, HttpStatusCode.InternalServerError, "INTERNAL_ERROR",
                "An unexpected error occurred.");
        }
    }

    private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode,
        string code, string message, object? details = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var payload = new
        {
            code,
            message,
            details,
            timestamp = DateTimeOffset.UtcNow,
            traceId = context.TraceIdentifier
        };

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }
}
