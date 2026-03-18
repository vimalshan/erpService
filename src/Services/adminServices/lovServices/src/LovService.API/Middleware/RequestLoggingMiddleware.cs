using System.Diagnostics;

namespace LovService.API.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                           ?? context.TraceIdentifier;

        logger.LogInformation(
            "HTTP {Method} {Path} started | CorrelationId: {CorrelationId}",
            context.Request.Method, context.Request.Path, correlationId);

        await next(context);

        sw.Stop();
        logger.LogInformation(
            "HTTP {Method} {Path} {StatusCode} completed in {ElapsedMs}ms | CorrelationId: {CorrelationId}",
            context.Request.Method, context.Request.Path, context.Response.StatusCode,
            sw.ElapsedMilliseconds, correlationId);
    }
}
