using System.Diagnostics;

namespace ApiGateway.Middleware;

public sealed class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    public const string HeaderName = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(HeaderName, out var correlationId)
            || string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("N");
        }

        context.Items["CorrelationId"] = correlationId.ToString();
        context.Response.Headers[HeaderName] = correlationId.ToString();

        Activity.Current?.SetTag("correlation.id", correlationId.ToString());

        using (logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId.ToString()! }))
        {
            await next(context);
        }
    }
}
