using System.Diagnostics;

namespace ScholarshipService.API.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault() ?? Guid.NewGuid().ToString();
        context.Response.Headers["X-Correlation-Id"] = correlationId;
        context.Items["CorrelationId"] = correlationId;

        logger.LogInformation("→ {Method} {Path} | CorrelationId: {CorrelationId}",
            context.Request.Method, context.Request.Path, correlationId);

        await next(context);

        sw.Stop();
        logger.LogInformation("← {Method} {Path} | {StatusCode} | {Elapsed}ms | CorrelationId: {CorrelationId}",
            context.Request.Method, context.Request.Path,
            context.Response.StatusCode, sw.ElapsedMilliseconds, correlationId);
    }
}
