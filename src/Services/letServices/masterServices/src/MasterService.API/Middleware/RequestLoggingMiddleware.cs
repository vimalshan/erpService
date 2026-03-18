using Serilog;

namespace MasterService.API.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        Log.Information("HTTP {Method} {Path} started", context.Request.Method, context.Request.Path);
        var sw = System.Diagnostics.Stopwatch.StartNew();
        await next(context);
        sw.Stop();
        Log.Information("HTTP {Method} {Path} responded {StatusCode} in {Elapsed}ms",
            context.Request.Method, context.Request.Path, context.Response.StatusCode, sw.ElapsedMilliseconds);
    }
}
