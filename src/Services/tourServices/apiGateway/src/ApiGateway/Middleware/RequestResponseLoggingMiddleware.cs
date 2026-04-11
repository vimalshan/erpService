using System.Diagnostics;
using Serilog;
using Serilog.Context;

namespace ApiGateway.Middleware;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;

            Log.Information(
                "Request: {Method} {Path}{QueryString} | Client: {ClientIp} | User-Agent: {UserAgent}",
                request.Method,
                request.Path,
                request.QueryString,
                context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                request.Headers.UserAgent.ToString());

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

                Log.Information(
                    "Response: {StatusCode} | Duration: {ElapsedMs}ms | Path: {Method} {Path}",
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds,
                    request.Method,
                    request.Path);

                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;
            }
        }
    }
}
