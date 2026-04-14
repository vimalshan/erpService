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
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? "N/A";
        var stopwatch = Stopwatch.StartNew();

        // Log request
        _logger.LogInformation(
            "[{CorrelationId}] --> {Method} {Path}{Query} | Client: {ClientIp} | Agent: {UserAgent}",
            correlationId,
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString,
            context.Connection.RemoteIpAddress,
            context.Request.Headers.UserAgent.FirstOrDefault() ?? "Unknown");

        // Capture the original response body stream
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

            // Log response
            _logger.LogInformation(
                "[{CorrelationId}] <-- {StatusCode} | {ElapsedMs}ms | {ContentType} | Size: {Size}B",
                correlationId,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                context.Response.ContentType ?? "N/A",
                responseBody.Length);

            if (context.Response.StatusCode >= 400)
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                var body = await new StreamReader(responseBody, Encoding.UTF8).ReadToEndAsync();
                var truncated = body.Length > 500 ? body[..500] + "..." : body;
                _logger.LogWarning(
                    "[{CorrelationId}] Error response body: {Body}",
                    correlationId, truncated);
            }

            // Copy the response body back to the original stream
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
