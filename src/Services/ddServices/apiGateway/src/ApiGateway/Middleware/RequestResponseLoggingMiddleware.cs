using System.Diagnostics;
using System.Text;

namespace ApiGateway.Middleware;

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
        var correlationId = context.Items["CorrelationId"]?.ToString() ?? "N/A";
        var sw = Stopwatch.StartNew();

        // ─── Log Request ────────────────────────────────────────────────────────
        var request = context.Request;
        _logger.LogInformation(
            "[{CorrelationId}] → {Method} {Path}{QueryString} | Client: {ClientIP} | Agent: {UserAgent}",
            correlationId,
            request.Method,
            request.Path,
            request.QueryString,
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            request.Headers.UserAgent.ToString().Length > 100
                ? request.Headers.UserAgent.ToString()[..100]
                : request.Headers.UserAgent.ToString());

        // Buffer the request body for logging (only for POST/PUT/PATCH, limited size)
        if (IsBodyMethod(request.Method) && request.ContentLength is > 0 and <= 10240)
        {
            request.EnableBuffering();
            var bodyBytes = new byte[Math.Min((int)request.ContentLength.Value, 10240)];
            var bytesRead = await request.Body.ReadAsync(bodyBytes);
            request.Body.Position = 0;

            if (bytesRead > 0)
            {
                var bodyPreview = Encoding.UTF8.GetString(bodyBytes, 0, bytesRead);
                if (bodyPreview.Length > 500) bodyPreview = bodyPreview[..500] + "...(truncated)";
                _logger.LogDebug("[{CorrelationId}] Request Body: {Body}", correlationId, bodyPreview);
            }
        }

        // ─── Capture Response ───────────────────────────────────────────────────
        var originalBody = context.Response.Body;
        using var responseBuffer = new MemoryStream();
        context.Response.Body = responseBuffer;

        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();
            context.Response.Body = originalBody;

            responseBuffer.Position = 0;
            await responseBuffer.CopyToAsync(originalBody);

            var statusCode = context.Response.StatusCode;
            var logLevel = statusCode switch
            {
                >= 500 => LogLevel.Error,
                >= 400 => LogLevel.Warning,
                _ => LogLevel.Information
            };

            _logger.Log(logLevel,
                "[{CorrelationId}] ← {StatusCode} | {Method} {Path} | {ElapsedMs}ms | Size: {ResponseSize}B",
                correlationId,
                statusCode,
                request.Method,
                request.Path,
                sw.ElapsedMilliseconds,
                responseBuffer.Length);
        }
    }

    private static bool IsBodyMethod(string method) =>
        method is "POST" or "PUT" or "PATCH";
}
