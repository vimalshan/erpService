using System.Net;
using System.Text.Json;

namespace ApiGateway.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                                ?? context.TraceIdentifier;

            logger.LogError(ex, "[{CorrelationId}] Unhandled exception in gateway", correlationId);

            context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = 502,
                title = "Gateway Error",
                detail = "An error occurred while processing your request through the API gateway.",
                correlationId,
                timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
