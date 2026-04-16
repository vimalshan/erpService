using System.Diagnostics;

namespace ApiGateway.Middleware;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    private static readonly HashSet<string> _sensitiveHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Authorization", "Cookie", "Set-Cookie", "X-Api-Key"
    };

    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = context.Items["CorrelationId"]?.ToString() ?? "N/A";

        await LogRequest(context, correlationId);

        var originalResponseBody = context.Response.Body;
        await using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            await LogResponse(context, correlationId, stopwatch.ElapsedMilliseconds, originalResponseBody);
            context.Response.Body = originalResponseBody;
        }
    }

    private async Task LogRequest(HttpContext context, string correlationId)
    {
        context.Request.EnableBuffering();

        var headers = context.Request.Headers
            .Where(h => !_sensitiveHeaders.Contains(h.Key))
            .ToDictionary(h => h.Key, h => h.Value.ToString());

        string requestBody = string.Empty;
        if (context.Request.ContentLength > 0 && context.Request.ContentLength < 8192)
        {
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Seek(0, SeekOrigin.Begin);
        }

        _logger.LogInformation(
            "HTTP Request  | CorrelationId: {CorrelationId} | {Method} {Path}{QueryString} | " +
            "Headers: {@Headers} | Body: {Body}",
            correlationId,
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString,
            headers,
            requestBody.Length > 500 ? requestBody[..500] + "...[truncated]" : requestBody);
    }

    private async Task LogResponse(
        HttpContext context,
        string correlationId,
        long elapsedMs,
        Stream originalResponseBody)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        string responseBody = string.Empty;
        if (context.Response.ContentLength is null or < 8192)
        {
            using var reader = new StreamReader(context.Response.Body, leaveOpen: true);
            responseBody = await reader.ReadToEndAsync();
        }

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        await context.Response.Body.CopyToAsync(originalResponseBody);

        _logger.LogInformation(
            "HTTP Response | CorrelationId: {CorrelationId} | Status: {StatusCode} | " +
            "Elapsed: {ElapsedMs}ms | Body: {Body}",
            correlationId,
            context.Response.StatusCode,
            elapsedMs,
            responseBody.Length > 500 ? responseBody[..500] + "...[truncated]" : responseBody);
    }
}

public static class RequestResponseLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
        => app.UseMiddleware<RequestResponseLoggingMiddleware>();
}
