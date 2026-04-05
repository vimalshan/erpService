namespace ERPGateway.Middleware;

/// <summary>
/// Ensures every request carries an <c>X-Correlation-ID</c> header.
/// If the client sends one it is reused; otherwise a new GUID is generated.
/// The value is:
///   • stored in <see cref="HttpContext.Items"/> so downstream middleware / handlers can read it
///   • echoed back in the response header
///   • pushed into the log scope so every log entry in the same request shares the ID
/// </summary>
public sealed class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-ID";

    private readonly RequestDelegate                  _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Reuse a client-supplied correlation ID or mint a new one.
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault()
                            ?? Guid.NewGuid().ToString("D");

        // Make available to the rest of the pipeline.
        context.Items[HeaderName] = correlationId;

        // Echo back so API consumers can correlate log entries.
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        // Push into log scope so Seq / Application Insights can group entries.
        using (_logger.BeginScope(
                   new Dictionary<string, object> { [HeaderName] = correlationId }))
        {
            await _next(context);
        }
    }
}
