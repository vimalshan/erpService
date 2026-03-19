using System.Diagnostics;

namespace MobileAppManagement.API.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var requestPath = context.Request.Path;
        var method = context.Request.Method;

        logger.LogInformation("Incoming {Method} {Path}", method, requestPath);

        await next(context);

        sw.Stop();
        logger.LogInformation("{Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
            method, requestPath, context.Response.StatusCode, sw.ElapsedMilliseconds);
    }
}
