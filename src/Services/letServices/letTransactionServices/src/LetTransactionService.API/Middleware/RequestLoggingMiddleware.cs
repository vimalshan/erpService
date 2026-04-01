using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LetTransactionService.API.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();
        context.Response.Headers["X-Correlation-ID"] = correlationId;

        var sw = Stopwatch.StartNew();
        logger.LogInformation("HTTP {Method} {Path} started. CorrelationId: {CorrelationId}",
            context.Request.Method, context.Request.Path, correlationId);

        await next(context);

        sw.Stop();
        logger.LogInformation("HTTP {Method} {Path} finished in {ElapsedMs}ms with status {StatusCode}. CorrelationId: {CorrelationId}",
            context.Request.Method, context.Request.Path, sw.ElapsedMilliseconds,
            context.Response.StatusCode, correlationId);
    }
}
