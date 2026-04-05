using System.Diagnostics;

namespace ERPGateway.Middleware;

/// <summary>
/// Logs each proxied request and response with timing information.
///
/// Inbound log  : method, path, correlation ID
/// Outbound log : method, path, HTTP status code, elapsed milliseconds, correlation ID
/// </summary>
public sealed class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate                            _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Items.TryGetValue("X-Correlation-ID", out var cid)
            ? cid?.ToString()
            : "N/A";

        var sw = Stopwatch.StartNew();

        _logger.LogInformation(
            "[GW ←] {Method} {Path}{Query} | CorrelationId={CorrelationId} | IP={IP}",
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString,
            correlationId,
            context.Connection.RemoteIpAddress);

        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();

            var level = context.Response.StatusCode >= 500
                ? LogLevel.Error
                : context.Response.StatusCode >= 400
                    ? LogLevel.Warning
                    : LogLevel.Information;

            _logger.Log(
                level,
                "[GW →] {Method} {Path} | Status={Status} | {Elapsed}ms | CorrelationId={CorrelationId}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds,
                correlationId);
        }
    }
}
