using System.Diagnostics;
using System.Text;

namespace LoanApiGateway.Middleware;

/// <summary>
/// Logs every proxied request and response including method, path,
/// status code, duration, and correlation ID.
/// </summary>
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Items.TryGetValue(CorrelationIdMiddleware.HeaderName, out var cid)
            ? cid?.ToString() ?? "N/A"
            : context.Request.Headers[CorrelationIdMiddleware.HeaderName].FirstOrDefault() ?? "N/A";

        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "[{CorrelationId}] ► {Method} {Scheme}://{Host}{Path}{QueryString}",
            correlationId,
            context.Request.Method,
            context.Request.Scheme,
            context.Request.Host,
            context.Request.Path,
            context.Request.QueryString);

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var level = context.Response.StatusCode >= 500
                ? LogLevel.Error
                : context.Response.StatusCode >= 400
                    ? LogLevel.Warning
                    : LogLevel.Information;

            _logger.Log(level,
                "[{CorrelationId}] ◄ {StatusCode} {Method} {Path} in {ElapsedMs}ms",
                correlationId,
                context.Response.StatusCode,
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds);
        }
    }
}

public static class RequestResponseLoggingExtensions
{
    public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
        => app.UseMiddleware<RequestResponseLoggingMiddleware>();
}
