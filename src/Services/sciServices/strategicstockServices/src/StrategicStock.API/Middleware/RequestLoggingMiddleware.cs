using System.Diagnostics;

namespace StrategicStock.API.Middleware;

public sealed class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        logger.LogInformation("Request {Method} {Path} started", context.Request.Method, context.Request.Path);

        await next(context);

        stopwatch.Stop();
        logger.LogInformation("Request {Method} {Path} completed in {ElapsedMs}ms with {StatusCode}",
            context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds, context.Response.StatusCode);
    }
}
