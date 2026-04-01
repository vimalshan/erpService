using System.Diagnostics;
using System.Text;

namespace ApiGateway.Middleware;

public sealed class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
{
    private static readonly HashSet<string> SensitiveHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Authorization", "Cookie", "Set-Cookie", "X-Api-Key"
    };

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Items["CorrelationId"]?.ToString() ?? "-";
        var sw = Stopwatch.StartNew();

        // ── Log Request ─────────────────────────────────────────────
        var request = context.Request;
        logger.LogInformation(
            "[{CorrelationId}] ► {Method} {Path}{Query} | Client={ClientIp} | ContentLength={ContentLength}",
            correlationId,
            request.Method,
            request.Path,
            request.QueryString,
            context.Connection.RemoteIpAddress,
            request.ContentLength ?? 0);

        if (logger.IsEnabled(LogLevel.Debug))
        {
            foreach (var header in request.Headers)
            {
                var value = SensitiveHeaders.Contains(header.Key) ? "***REDACTED***" : header.Value.ToString();
                logger.LogDebug("[{CorrelationId}]   Header: {Key}={Value}", correlationId, header.Key, value);
            }
        }

        // ── Wrap response stream to capture status ──────────────────
        var originalBody = context.Response.Body;
        await using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            await next(context);
        }
        finally
        {
            sw.Stop();
            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBody);
            context.Response.Body = originalBody;

            var statusCode = context.Response.StatusCode;
            var level = statusCode >= 500 ? LogLevel.Error
                      : statusCode >= 400 ? LogLevel.Warning
                      : LogLevel.Information;

            logger.Log(level,
                "[{CorrelationId}] ◄ {Method} {Path} → {StatusCode} | {ElapsedMs}ms | Size={ResponseSize}",
                correlationId,
                request.Method,
                request.Path,
                statusCode,
                sw.ElapsedMilliseconds,
                memoryStream.Length);
        }
    }
}
