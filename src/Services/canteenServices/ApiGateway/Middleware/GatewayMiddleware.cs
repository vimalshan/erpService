namespace ApiGateway.Middleware;

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
        var start = DateTime.UtcNow;
        _logger.LogInformation("[Gateway] {Method} {Path}", context.Request.Method, context.Request.Path);

        await _next(context);

        var elapsed = (DateTime.UtcNow - start).TotalMilliseconds;
        _logger.LogInformation("[Gateway] {StatusCode} for {Method} {Path} ({Elapsed:F1}ms)",
            context.Response.StatusCode, context.Request.Method, context.Request.Path, elapsed);
    }
}

public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeader = "X-Correlation-ID";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey(CorrelationIdHeader))
        {
            context.Request.Headers.Append(CorrelationIdHeader, Guid.NewGuid().ToString());
        }

        var correlationId = context.Request.Headers[CorrelationIdHeader].ToString();
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Append(CorrelationIdHeader, correlationId);
            return Task.CompletedTask;
        });

        await _next(context);
    }
}
