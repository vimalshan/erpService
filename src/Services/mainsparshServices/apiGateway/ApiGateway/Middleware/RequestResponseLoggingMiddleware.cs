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

        // Log Request
        var method = context.Request.Method;
        var path = context.Request.Path + context.Request.QueryString;
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        _logger.LogInformation("[{CorrelationId}] => {Method} {Path} from {ClientIp}",
            correlationId, method, path, clientIp);

        // Capture request body for POST/PUT/PATCH (limit to 4KB for logging)
        if (context.Request.ContentLength > 0 && context.Request.ContentLength <= 4096
            && (method == "POST" || method == "PUT" || method == "PATCH"))
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            _logger.LogDebug("[{CorrelationId}] Request Body: {Body}", correlationId, body);
        }

        // Capture response
        var originalBodyStream = context.Response.Body;
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;
            var elapsed = stopwatch.ElapsedMilliseconds;

            // Log response body for errors (limit to 4KB)
            if (statusCode >= 400 && responseBody.Length > 0 && responseBody.Length <= 4096)
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(responseBody, Encoding.UTF8, leaveOpen: true);
                var errorBody = await reader.ReadToEndAsync();
                _logger.LogWarning("[{CorrelationId}] <= {StatusCode} in {Elapsed}ms | Error: {ErrorBody}",
                    correlationId, statusCode, elapsed, errorBody);
            }
            else
            {
                _logger.LogInformation("[{CorrelationId}] <= {StatusCode} in {Elapsed}ms",
                    correlationId, statusCode, elapsed);
            }

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }
    }
}
