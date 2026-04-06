using System.Net;
using System.Text.Json;

namespace ApiGateway.Middleware;

/// <summary>
/// Global exception handling middleware. Catches unhandled exceptions and returns
/// a structured JSON error response with correlation ID.
/// </summary>
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
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Request was cancelled: {Path}", context.Request.Path);
            await WriteErrorResponse(context, HttpStatusCode.GatewayTimeout,
                "The downstream service did not respond in time.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Downstream service error for {Path}", context.Request.Path);
            await WriteErrorResponse(context, HttpStatusCode.BadGateway,
                "The downstream service is unavailable.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception for {Path}", context.Request.Path);
            await WriteErrorResponse(context, HttpStatusCode.InternalServerError,
                "An internal gateway error occurred.");
        }
    }

    private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode, string message)
    {
        if (context.Response.HasStarted) return;

        var correlationId = context.Items["CorrelationId"]?.ToString() ?? "unknown";
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var error = new
        {
            status = (int)statusCode,
            title = statusCode.ToString(),
            detail = message,
            correlationId,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(error, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
