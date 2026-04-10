using System.Diagnostics;
using System.Text;

namespace ApiGateway.Middleware;

public class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                            ?? context.TraceIdentifier;

        var stopwatch = Stopwatch.StartNew();

        // Log Request
        var requestBody = string.Empty;
        if (context.Request.ContentLength > 0 && context.Request.ContentLength < 10240)
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        logger.LogInformation(
            "[{CorrelationId}] => {Method} {Path}{Query} | Client: {ClientIp} | Body: {Body}",
            correlationId,
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString,
            context.Connection.RemoteIpAddress,
            requestBody.Length > 500 ? requestBody[..500] + "..." : requestBody);

        // Capture Response
        var originalBody = context.Response.Body;
        using var memStream = new MemoryStream();
        context.Response.Body = memStream;

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();

            memStream.Position = 0;
            var responseBody = await new StreamReader(memStream).ReadToEndAsync();
            memStream.Position = 0;
            await memStream.CopyToAsync(originalBody);
            context.Response.Body = originalBody;

            var level = context.Response.StatusCode >= 500 ? LogLevel.Error
                      : context.Response.StatusCode >= 400 ? LogLevel.Warning
                      : LogLevel.Information;

            logger.Log(level,
                "[{CorrelationId}] <= {StatusCode} | {ElapsedMs}ms | Size: {Size}B | Body: {Body}",
                correlationId,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                responseBody.Length,
                responseBody.Length > 500 ? responseBody[..500] + "..." : responseBody);
        }
    }
}
