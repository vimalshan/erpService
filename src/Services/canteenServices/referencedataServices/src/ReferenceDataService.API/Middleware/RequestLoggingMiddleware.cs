using System.Diagnostics;

namespace ReferenceDataService.API.Middleware;

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

        _logger.LogInformation("Incoming request: {Method} {Path}", context.Request.Method, context.Request.Path);

        await _next(context);

        sw.Stop();

        _logger.LogInformation("Response: {StatusCode} in {ElapsedMs}ms for {Method} {Path}",
            context.Response.StatusCode, sw.ElapsedMilliseconds,
            context.Request.Method, context.Request.Path);
    }
}
