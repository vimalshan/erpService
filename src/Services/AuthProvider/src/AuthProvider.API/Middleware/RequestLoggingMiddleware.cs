using System.Diagnostics;

namespace AuthProvider.API.Middleware;

/// <summary>
/// Request Logging middleware – logs request/response details with timing.
/// Works alongside Serilog request logging for structured log output.
/// </summary>
public sealed class RequestLoggingMiddleware
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
        var correlationId = context.Items["CorrelationId"]?.ToString() ?? "N/A";

        _logger.LogInformation(
            "[{CorrelationId}] ► {Method} {Path}{Query}",
            correlationId,
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString);

        await _next(context);

        sw.Stop();
        var level = context.Response.StatusCode >= 500 ? LogLevel.Error
                    : context.Response.StatusCode >= 400 ? LogLevel.Warning
                    : LogLevel.Information;

        _logger.Log(level,
            "[{CorrelationId}] ◄ {Method} {Path} → {StatusCode} ({Elapsed}ms)",
            correlationId,
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            sw.ElapsedMilliseconds);
    }
}
