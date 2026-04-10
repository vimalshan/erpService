using System.Net;
using System.Text.Json;

namespace SparshApiGateway.Middleware;

/// <summary>
/// Global exception handler for the gateway. Catches unhandled exceptions
/// and returns structured JSON error responses.
/// </summary>
public class GatewayExceptionMiddleware(RequestDelegate next, ILogger<GatewayExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (HttpRequestException ex)
        {
            var correlationId = context.Items["CorrelationId"]?.ToString();
            logger.LogError(ex, "Downstream service unavailable | CorrelationId: {CorrelationId}", correlationId);

            context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = "Downstream service is unavailable.",
                correlationId,
                statusCode = 502
            }));
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            var correlationId = context.Items["CorrelationId"]?.ToString();
            logger.LogError(ex, "Request timeout | CorrelationId: {CorrelationId}", correlationId);

            context.Response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = "Request timed out. Please try again.",
                correlationId,
                statusCode = 504
            }));
        }
        catch (Exception ex)
        {
            var correlationId = context.Items["CorrelationId"]?.ToString();
            logger.LogError(ex, "Unhandled gateway error | CorrelationId: {CorrelationId}", correlationId);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = "An unexpected gateway error occurred.",
                correlationId,
                statusCode = 500
            }));
        }
    }
}
