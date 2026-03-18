using System.Diagnostics;

namespace AuditService.API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();
        context.Response.Headers["X-Correlation-ID"] = correlationId;

        _logger.LogInformation(
            "[{CorrelationId}] {Method} {Path} started",
            correlationId, context.Request.Method, context.Request.Path);

        await _next(context);

        sw.Stop();
        _logger.LogInformation(
            "[{CorrelationId}] {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
            correlationId, context.Request.Method, context.Request.Path,
            context.Response.StatusCode, sw.ElapsedMilliseconds);
    }
}
