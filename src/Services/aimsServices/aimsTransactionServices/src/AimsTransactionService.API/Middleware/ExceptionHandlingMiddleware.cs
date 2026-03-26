using FluentValidation;
using System.Text.Json;

namespace AimsTransactionService.API.Middleware;

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
            logger.LogWarning("Validation failed: {Errors}", ex.Errors);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";

            var problem = new
            {
                Type = "https://tools.ietf.org/html/rfc7807",
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors occurred.",
                Errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
        catch (System.Collections.Generic.KeyNotFoundException ex)
        {
            logger.LogWarning("Resource not found: {Message}", ex.Message);
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/problem+json";
            var problem = new { Title = "Not Found", Status = StatusCodes.Status404NotFound, Detail = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning("Business rule violation: {Message}", ex.Message);
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            context.Response.ContentType = "application/problem+json";
            var problem = new { Title = "Business Rule Violation", Status = StatusCodes.Status422UnprocessableEntity, Detail = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";
            var problem = new { Title = "Internal Server Error", Status = StatusCodes.Status500InternalServerError, Detail = "An unexpected error occurred." };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
