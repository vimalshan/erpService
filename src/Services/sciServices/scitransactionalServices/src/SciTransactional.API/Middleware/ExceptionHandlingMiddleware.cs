using System.Net;
using System.Text.Json;
using FluentValidation;

namespace SciTransactional.API.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation failed");
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { errors }));
            }
        }
        catch (System.Collections.Generic.KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Resource not found");
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(new { error = "An internal error occurred." }));
            }
        }
    }
}
