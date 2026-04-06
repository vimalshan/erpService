using System.Diagnostics;
using System.Text;

namespace ApiGateway.Middleware;

/// <summary>
/// Captures and logs HTTP request/response details with timing information.
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
        var correlationId = context.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();
        var stopwatch = Stopwatch.StartNew();

        // Log Request
        var request = context.Request;
        var requestBody = string.Empty;

        if (request.ContentLength > 0 && request.ContentLength < 10240) // Max 10KB body log
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            request.Body.Position = 0;
        }

        _logger.LogInformation(
            "[{CorrelationId}] ► REQUEST {Method} {Path}{QueryString} | Client: {ClientIp} | ContentType: {ContentType} | ContentLength: {ContentLength}",
            correlationId,
            request.Method,
            request.Path,
            request.QueryString,
            context.Connection.RemoteIpAddress,
            request.ContentType,
            request.ContentLength);

        if (!string.IsNullOrEmpty(requestBody))
        {
            _logger.LogDebug("[{CorrelationId}] Request Body: {Body}", correlationId, requestBody);
        }

        // Capture Response
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

            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = string.Empty;
            if (responseBody.Length < 10240) // Max 10KB response log
            {
                responseText = await new StreamReader(responseBody).ReadToEndAsync();
                responseBody.Seek(0, SeekOrigin.Begin);
            }

            var statusCode = context.Response.StatusCode;
            var logLevel = statusCode >= 500 ? LogLevel.Error
                         : statusCode >= 400 ? LogLevel.Warning
                         : LogLevel.Information;

            _logger.Log(logLevel,
                "[{CorrelationId}] ◄ RESPONSE {StatusCode} | Duration: {ElapsedMs}ms | ContentType: {ContentType}",
                correlationId,
                statusCode,
                stopwatch.ElapsedMilliseconds,
                context.Response.ContentType);

            if (!string.IsNullOrEmpty(responseText) && logLevel >= LogLevel.Warning)
            {
                _logger.LogDebug("[{CorrelationId}] Response Body: {Body}", correlationId, responseText);
            }

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
