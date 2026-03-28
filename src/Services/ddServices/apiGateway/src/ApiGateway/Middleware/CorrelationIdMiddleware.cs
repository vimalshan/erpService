using System.Diagnostics;

namespace ApiGateway.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationIdHeader = "X-Correlation-Id";

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        // Extract or generate correlation ID
        if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId)
            || string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("N");
        }

        var id = correlationId.ToString();

        // Set on the context for downstream use
        context.Items["CorrelationId"] = id;
        context.TraceIdentifier = id;

        // Add to response headers
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[CorrelationIdHeader] = id;
            return Task.CompletedTask;
        });

        // Push to Serilog LogContext
        using (Serilog.Context.LogContext.PushProperty("CorrelationId", id))
        {
            // Forward to downstream services
            context.Request.Headers[CorrelationIdHeader] = id;
            await _next(context);
        }
    }
}
