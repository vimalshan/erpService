namespace MobileExpenseManagement.API.Middleware;

/// <summary>
/// Middleware for request/response logging
/// </summary>
public class RequestLoggingMiddleware
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
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var originalBodyStream = context.Response.Body;

        try
        {
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                _logger.LogInformation(
                    $"HTTP {context.Request.Method} {context.Request.Path.Value} STARTED");

                await _next(context);

                stopwatch.Stop();

                _logger.LogInformation(
                    $"HTTP {context.Request.Method} {context.Request.Path.Value} COMPLETED - Status: {context.Response.StatusCode} - Duration: {stopwatch.ElapsedMilliseconds}ms");

                responseBody.Position = 0;
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                $"HTTP {context.Request.Method} {context.Request.Path.Value} FAILED - Duration: {stopwatch.ElapsedMilliseconds}ms");
            throw;
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }
}
