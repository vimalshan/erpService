using System.Diagnostics;

namespace ApiGateway.API.Middleware;

public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestPath = context.Request.Path;
        var method = context.Request.Method;

        _logger.LogInformation("Gateway request: {Method} {Path}", method, requestPath);

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;
            var elapsed = stopwatch.ElapsedMilliseconds;

            if (elapsed > 3000)
            {
                _logger.LogWarning(
                    "Gateway slow response: {Method} {Path} responded {StatusCode} in {Elapsed}ms",
                    method, requestPath, statusCode, elapsed);
            }
            else
            {
                _logger.LogInformation(
                    "Gateway response: {Method} {Path} responded {StatusCode} in {Elapsed}ms",
                    method, requestPath, statusCode, elapsed);
            }
        }
    }
}
