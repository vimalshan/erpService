using System.Net;
using System.Text.Json;
using FluentValidation;

namespace MobileAppManagement.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await HandleExceptionAsync(context, (int)HttpStatusCode.BadRequest, "Validation Error",
                JsonSerializer.Serialize(new
                {
                    title = "Validation Error",
                    status = 400,
                    errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                }));
            logger.LogWarning("Validation failed: {Errors}", ex.Errors);
        }
        catch (System.Collections.Generic.KeyNotFoundException ex)
        {
            await HandleExceptionAsync(context, (int)HttpStatusCode.NotFound, "Not Found",
                JsonSerializer.Serialize(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = ex.Message
                }));
            logger.LogWarning("Resource not found: {Message}", ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleExceptionAsync(context, (int)HttpStatusCode.Unauthorized, "Unauthorized",
                JsonSerializer.Serialize(new
                {
                    title = "Unauthorized",
                    status = 401,
                    detail = ex.Message
                }));
            logger.LogWarning("Unauthorized access: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            var correlationId = context.Items["CorrelationId"]?.ToString() ?? "N/A";
            await HandleExceptionAsync(context, (int)HttpStatusCode.InternalServerError, "Internal Server Error",
                JsonSerializer.Serialize(new
                {
                    title = "Internal Server Error",
                    status = 500,
                    detail = "An unexpected error occurred.",
                    correlationId = correlationId
                }));
            logger.LogError(ex, "Unhandled exception occurred [CorrelationId: {CorrelationId}]", correlationId);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, int statusCode, string contentType, string responseBody)
    {
        // Check if response has already started before modifying it
        if (context.Response.HasStarted)
        {
            return Task.CompletedTask;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(responseBody);
    }
}
