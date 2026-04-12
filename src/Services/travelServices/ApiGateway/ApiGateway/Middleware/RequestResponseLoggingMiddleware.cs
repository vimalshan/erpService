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
        var stopwatch = Stopwatch.StartNew();

        // Log request
        _logger.LogInformation(
            "[{CorrelationId}] --> {Method} {Path}{QueryString} | Client: {ClientIp} | Agent: {UserAgent}",
            correlationId,
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString,
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            context.Request.Headers.UserAgent.ToString());

        // Capture response body for logging
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseSize = context.Response.Body.Length;
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation(
                "[{CorrelationId}] <-- {StatusCode} | {ElapsedMs}ms | {ResponseSize} bytes | {Method} {Path}",
                correlationId,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                responseSize,
                context.Request.Method,
                context.Request.Path);

            if (context.Response.StatusCode >= 500)
            {
                _logger.LogError(
                    "[{CorrelationId}] Server error {StatusCode} on {Method} {Path}",
                    correlationId,
                    context.Response.StatusCode,
                    context.Request.Method,
                    context.Request.Path);
            }

            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }
    }
}
