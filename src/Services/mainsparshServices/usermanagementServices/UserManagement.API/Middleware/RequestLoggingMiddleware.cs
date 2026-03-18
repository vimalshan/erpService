using System.Diagnostics;

namespace UserManagement.API.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var requestId = context.TraceIdentifier;

        logger.LogInformation(
            "Request started {Method} {Path} RequestId={RequestId}",
            context.Request.Method, context.Request.Path, requestId);

        await next(context);
        sw.Stop();

        logger.LogInformation(
            "Request finished {Method} {Path} {StatusCode} in {Elapsed}ms RequestId={RequestId}",
            context.Request.Method, context.Request.Path,
            context.Response.StatusCode, sw.ElapsedMilliseconds, requestId);
    }
}
