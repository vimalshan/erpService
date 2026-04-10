using System.Diagnostics;

namespace SSC.ApiGateway.Middleware;

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
        var request = context.Request;

        _logger.LogInformation(
            "Request {Method} {Path}{QueryString} | CorrelationId: {CorrelationId} | ClientIP: {ClientIP} | ContentType: {ContentType} | ContentLength: {ContentLength}",
            request.Method,
            request.Path,
            request.QueryString,
            correlationId,
            context.Connection.RemoteIpAddress,
            request.ContentType,
            request.ContentLength);

        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        var statusCode = context.Response.StatusCode;
        var logLevel = statusCode >= 500 ? LogLevel.Error
                     : statusCode >= 400 ? LogLevel.Warning
                     : LogLevel.Information;

        _logger.Log(logLevel,
            "Response {StatusCode} for {Method} {Path} | CorrelationId: {CorrelationId} | Duration: {Duration}ms",
            statusCode,
            request.Method,
            request.Path,
            correlationId,
            stopwatch.ElapsedMilliseconds);
    }
}
