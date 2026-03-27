using System.Net;
using System.Text.Json;

namespace ApiGateway.API.Middleware;

public sealed class GatewayExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GatewayExceptionMiddleware> _logger;

    public GatewayExceptionMiddleware(RequestDelegate next, ILogger<GatewayExceptionMiddleware> logger)
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
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Downstream service unavailable for {Path}", context.Request.Path);
            await WriteErrorAsync(context, HttpStatusCode.ServiceUnavailable,
                "The requested service is currently unavailable. Please try again later.");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            _logger.LogError(ex, "Downstream service timeout for {Path}", context.Request.Path);
            await WriteErrorAsync(context, HttpStatusCode.GatewayTimeout,
                "The downstream service did not respond in time.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled gateway exception for {Path}", context.Request.Path);
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError,
                "An unexpected error occurred in the API gateway.");
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var error = new
        {
            status = (int)statusCode,
            title = statusCode.ToString(),
            message,
            traceId = context.TraceIdentifier,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(error, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
