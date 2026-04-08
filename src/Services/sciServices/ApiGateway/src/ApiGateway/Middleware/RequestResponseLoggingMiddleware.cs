using System.Diagnostics;

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

        _logger.LogInformation(
            "[{CorrelationId}] Request: {Method} {Path}{QueryString} from {RemoteIp}",
            correlationId,
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString,
            context.Connection.RemoteIpAddress);

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
            await responseBody.CopyToAsync(originalBodyStream);

            _logger.LogInformation(
                "[{CorrelationId}] Response: {StatusCode} in {ElapsedMs}ms for {Method} {Path}",
                correlationId,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                context.Request.Method,
                context.Request.Path);

            if (context.Response.StatusCode >= 500)
            {
                _logger.LogError(
                    "[{CorrelationId}] Server error {StatusCode} for {Method} {Path}",
                    correlationId,
                    context.Response.StatusCode,
                    context.Request.Method,
                    context.Request.Path);
            }
        }
    }
}

public static class RequestResponseLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        => builder.UseMiddleware<RequestResponseLoggingMiddleware>();
}
