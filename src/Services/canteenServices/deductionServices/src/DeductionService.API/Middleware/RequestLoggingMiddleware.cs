namespace DeductionService.API.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = Guid.NewGuid().ToString("N")[..8];
        using var scope = logger.BeginScope(new Dictionary<string, object> { ["TraceId"] = traceId });

        logger.LogInformation(
            "[{TraceId}] --> {Method} {Path}{Query}",
            traceId, context.Request.Method, context.Request.Path, context.Request.QueryString);

        var sw = System.Diagnostics.Stopwatch.StartNew();
        await next(context);
        sw.Stop();

        logger.LogInformation(
            "[{TraceId}] <-- {StatusCode} ({Elapsed}ms)",
            traceId, context.Response.StatusCode, sw.ElapsedMilliseconds);
    }
}
