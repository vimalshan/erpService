using System.Diagnostics;

namespace ProductService.API.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var method = context.Request.Method;
        var path = context.Request.Path;

        logger.LogInformation("→ {Method} {Path}", method, path);

        await next(context);

        sw.Stop();
        logger.LogInformation("← {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
            method, path, context.Response.StatusCode, sw.ElapsedMilliseconds);
    }
}
