using System.Net;
using System.Text.Json;
using FluentValidation;

namespace VehicleTracking.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Let HotChocolate handle its own errors
        if (context.Request.Path.StartsWithSegments("/graphql"))
        {
            await next(context);
            return;
        }

        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error occurred");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { Errors = errors }));
        }
        catch (System.Collections.Generic.KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Resource not found");
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { Error = ex.Message }));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { Error = "An internal server error occurred." }));
        }
    }
}

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation("Request: {Method} {Path} from {IP}",
            context.Request.Method, context.Request.Path, context.Connection.RemoteIpAddress);

        var sw = System.Diagnostics.Stopwatch.StartNew();
        await next(context);
        sw.Stop();

        logger.LogInformation("Response: {StatusCode} in {ElapsedMs}ms",
            context.Response.StatusCode, sw.ElapsedMilliseconds);
    }
}
