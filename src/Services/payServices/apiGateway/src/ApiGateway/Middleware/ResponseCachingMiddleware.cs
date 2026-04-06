using Microsoft.Extensions.Caching.Memory;

namespace ApiGateway.Middleware;

/// <summary>
/// In-memory response caching middleware for GET requests.
/// Caches responses by path + query for configurable TTL.
/// </summary>
public class ResponseCachingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ResponseCachingMiddleware> _logger;
    private readonly int _defaultTtlSeconds;

    public ResponseCachingMiddleware(RequestDelegate next, IMemoryCache cache,
        ILogger<ResponseCachingMiddleware> logger, IConfiguration configuration)
    {
        _next = next;
        _cache = cache;
        _logger = logger;
        _defaultTtlSeconds = configuration.GetValue("Caching:DefaultTtlSeconds", 30);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Only cache GET requests
        if (!HttpMethods.IsGet(context.Request.Method))
        {
            await _next(context);
            return;
        }

        // Skip caching for auth endpoints and health checks
        var path = context.Request.Path.Value ?? "";
        if (path.Contains("/auth/", StringComparison.OrdinalIgnoreCase) ||
            path.Contains("/health", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var cacheKey = $"response:{context.Request.Path}{context.Request.QueryString}";

        if (_cache.TryGetValue(cacheKey, out CachedResponse? cached) && cached != null)
        {
            _logger.LogDebug("Cache HIT for {Path}", context.Request.Path);
            context.Response.StatusCode = cached.StatusCode;
            context.Response.ContentType = cached.ContentType ?? "application/json";
            context.Response.Headers["X-Cache"] = "HIT";
            await context.Response.Body.WriteAsync(cached.Body);
            return;
        }

        _logger.LogDebug("Cache MISS for {Path}", context.Request.Path);

        var originalBody = context.Response.Body;
        using var memStream = new MemoryStream();
        context.Response.Body = memStream;

        await _next(context);

        memStream.Seek(0, SeekOrigin.Begin);
        var body = memStream.ToArray();

        // Only cache successful responses
        if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
        {
            var cachedResponse = new CachedResponse
            {
                StatusCode = context.Response.StatusCode,
                ContentType = context.Response.ContentType,
                Body = body
            };

            _cache.Set(cacheKey, cachedResponse, TimeSpan.FromSeconds(_defaultTtlSeconds));
            context.Response.Headers["X-Cache"] = "MISS";
        }

        memStream.Seek(0, SeekOrigin.Begin);
        await memStream.CopyToAsync(originalBody);
        context.Response.Body = originalBody;
    }

    private class CachedResponse
    {
        public int StatusCode { get; set; }
        public string? ContentType { get; set; }
        public byte[] Body { get; set; } = [];
    }
}
