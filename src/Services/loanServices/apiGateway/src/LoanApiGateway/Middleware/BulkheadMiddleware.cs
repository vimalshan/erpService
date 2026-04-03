using System.Collections.Concurrent;
using System.Threading;

namespace LoanApiGateway.Middleware;

/// <summary>
/// Bulkhead isolation: limits the number of concurrent requests per route cluster.
/// Requests that exceed the concurrency limit receive 429 Too Many Requests.
/// </summary>
public class BulkheadMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<BulkheadMiddleware> _logger;
    private readonly int _maxConcurrentRequests;

    // Per-cluster semaphore pool
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new();

    public BulkheadMiddleware(
        RequestDelegate next,
        ILogger<BulkheadMiddleware> logger,
        IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        _maxConcurrentRequests = configuration.GetValue("Gateway:Bulkhead:MaxConcurrentRequests", 50);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Derive a bucket key from the first path segment (cluster name)
        var bucketKey = GetBucketKey(context.Request.Path);
        var semaphore = _semaphores.GetOrAdd(bucketKey,
            _ => new SemaphoreSlim(_maxConcurrentRequests, _maxConcurrentRequests));

        var acquired = semaphore.Wait(0);  // non-blocking try
        if (!acquired)
        {
            var correlationId = context.Items.TryGetValue(CorrelationIdMiddleware.HeaderName, out var c)
                ? c?.ToString() : "N/A";

            _logger.LogWarning(
                "[{CorrelationId}] Bulkhead rejected request to bucket '{Bucket}' — too many concurrent requests",
                correlationId, bucketKey);

            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.Headers["Retry-After"] = "5";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Too many concurrent requests. Please retry after a short delay.",
                bucket = bucketKey
            });
            return;
        }

        try
        {
            await _next(context);
        }
        finally
        {
            semaphore.Release();
        }
    }

    private static string GetBucketKey(PathString path)
    {
        var segments = path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries);
        return segments?.Length > 0 ? segments[0].ToLowerInvariant() : "default";
    }
}

public static class BulkheadMiddlewareExtensions
{
    public static IApplicationBuilder UseBulkheadIsolation(this IApplicationBuilder app)
        => app.UseMiddleware<BulkheadMiddleware>();
}
