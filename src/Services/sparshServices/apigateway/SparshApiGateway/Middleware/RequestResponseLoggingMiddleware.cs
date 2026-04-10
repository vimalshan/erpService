using System.Diagnostics;
using System.Text;

namespace SparshApiGateway.Middleware;

/// <summary>
/// Logs incoming requests and outgoing responses with timing, status codes , and correlation IDs.
/// </summary>
public class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = context.Items["CorrelationId"]?.ToString() ?? "N/A";
        var requestMethod = context.Request.Method;
        var requestPath = context.Request.Path;
        var queryString = context.Request.QueryString;
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var userAgent = context.Request.Headers.UserAgent.FirstOrDefault() ?? "unknown";

        logger.LogInformation(
            "=> REQ  {CorrelationId} | {Method} {Path}{Query} | IP: {ClientIp} | Agent: {UserAgent}",
            correlationId, requestMethod, requestPath, queryString, clientIp, userAgent);

        // Capture the original response body stream
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;
            var elapsed = stopwatch.ElapsedMilliseconds;

            var logLevel = statusCode switch
            {
                >= 500 => LogLevel.Error,
                >= 400 => LogLevel.Warning,
                _ => LogLevel.Information
            };

            logger.Log(logLevel,
                "<= RESP {CorrelationId} | {Method} {Path} | Status: {StatusCode} | {ElapsedMs}ms",
                correlationId, requestMethod, requestPath, statusCode, elapsed);

            // Copy the response body back
            responseBody.Position = 0;
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
