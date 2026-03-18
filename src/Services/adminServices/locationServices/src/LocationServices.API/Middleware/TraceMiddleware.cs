namespace LocationServices.API.Middleware;

/// <summary>
/// Injects a unique X-Correlation-ID header into every request/response pair.
/// Downstream services should propagate this header for distributed tracing.
/// </summary>
public sealed class CorrelationIdMiddleware
{
    private const string Header = "X-Correlation-ID";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext ctx)
    {
        var correlationId = ctx.Request.Headers.TryGetValue(Header, out var existing) && !string.IsNullOrWhiteSpace(existing)
            ? existing.ToString()
            : Guid.NewGuid().ToString();

        ctx.Items[Header]      = correlationId;
        ctx.Response.Headers[Header] = correlationId;

        await _next(ctx);
    }
}

/// <summary>
/// Logs every HTTP request with method, path, status code, and elapsed time.
/// </summary>
public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            await _next(ctx);
        }
        finally
        {
            sw.Stop();
            var correlationId = ctx.Items.TryGetValue("X-Correlation-ID", out var c) ? c : "-";
            _logger.LogInformation(
                "[HTTP] {Method} {Path} → {Status} | {Ms}ms | CorrelationId={CorrelationId}",
                ctx.Request.Method,
                ctx.Request.Path,
                ctx.Response.StatusCode,
                sw.ElapsedMilliseconds,
                correlationId);
        }
    }
}
