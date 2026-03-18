using System.Diagnostics;

namespace CourseService.API.Middleware;

/// <summary>
/// Adds request correlation ID header and logs request/response timing.
/// </summary>
public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();
        context.Response.Headers["X-Correlation-ID"] = correlationId;

        var sw = Stopwatch.StartNew();
        logger.LogInformation("HTTP {Method} {Path} started. CorrelationId: {CorrelationId}",
            context.Request.Method, context.Request.Path, correlationId);

        await next(context);

        sw.Stop();
        logger.LogInformation("HTTP {Method} {Path} finished in {ElapsedMs}ms with status {StatusCode}. CorrelationId: {CorrelationId}",
            context.Request.Method, context.Request.Path, sw.ElapsedMilliseconds,
            context.Response.StatusCode, correlationId);
    }
}
