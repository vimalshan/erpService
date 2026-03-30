using Hr.ApiGateway.Telemetry;

namespace Hr.ApiGateway.Middleware;

public sealed class RequestResponseLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestResponseLoggingMiddleware> logger,
    GatewayMetrics metrics)
{
    public async Task Invoke(HttpContext context)
    {
        var correlationId = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? "unknown";
        var startedAt = DateTimeOffset.UtcNow;

        logger.LogInformation(
            "Gateway request started. CorrelationId={CorrelationId} Method={Method} Path={Path}",
            correlationId,
            context.Request.Method,
            context.Request.Path);

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            metrics.Track(context.Response.StatusCode, stopwatch.ElapsedMilliseconds);

            logger.LogInformation(
                "Gateway response completed. CorrelationId={CorrelationId} StatusCode={StatusCode} DurationMs={DurationMs} StartedAt={StartedAt}",
                correlationId,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                startedAt);
        }
    }
}
