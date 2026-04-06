using System.Threading.RateLimiting;

namespace ApiGateway.Extensions;

/// <summary>
/// Configures rate limiting with Fixed Window, Sliding Window, and Token Bucket policies.
/// Applied globally and per-client (identified by X-Client-Id header or IP).
/// </summary>
public static class RateLimitingExtensions
{
    public static IServiceCollection AddGatewayRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var fixedConfig = configuration.GetSection("RateLimiting:Fixed");
        var slidingConfig = configuration.GetSection("RateLimiting:Sliding");
        var tokenConfig = configuration.GetSection("RateLimiting:Token");

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, cancellationToken) =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                var correlationId = context.HttpContext.Items["CorrelationId"]?.ToString() ?? "unknown";

                logger.LogWarning("[{CorrelationId}] Rate limit exceeded for {Path} from {ClientIp}",
                    correlationId,
                    context.HttpContext.Request.Path,
                    context.HttpContext.Connection.RemoteIpAddress);

                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsJsonAsync(new
                {
                    status = 429,
                    title = "Too Many Requests",
                    detail = "Rate limit exceeded. Please try again later.",
                    correlationId,
                    retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                        ? retryAfter.TotalSeconds : 60
                }, cancellationToken);
            };

            // Fixed Window — general API rate limit
            options.AddPolicy("fixed", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: "fixed-global",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        Window = TimeSpan.FromSeconds(fixedConfig.GetValue("Window", 60)),
                        PermitLimit = fixedConfig.GetValue("PermitLimit", 100),
                        QueueLimit = fixedConfig.GetValue("QueueLimit", 10),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }));

            // Sliding Window — smoother rate limiting
            options.AddPolicy("sliding", httpContext =>
                RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: "sliding-global",
                    factory: _ => new SlidingWindowRateLimiterOptions
                    {
                        Window = TimeSpan.FromSeconds(slidingConfig.GetValue("Window", 60)),
                        SegmentsPerWindow = slidingConfig.GetValue("SegmentsPerWindow", 6),
                        PermitLimit = slidingConfig.GetValue("PermitLimit", 100),
                        QueueLimit = 5,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }));

            // Token Bucket — burst-friendly limiting
            options.AddPolicy("token", httpContext =>
                RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: "token-global",
                    factory: _ => new TokenBucketRateLimiterOptions
                    {
                        ReplenishmentPeriod = TimeSpan.FromSeconds(tokenConfig.GetValue("ReplenishmentPeriod", 10)),
                        TokensPerPeriod = tokenConfig.GetValue("TokensPerPeriod", 20),
                        TokenLimit = tokenConfig.GetValue("TokenLimit", 100),
                        QueueLimit = 10,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }));

            // Per-client rate limiting based on X-Client-Id or IP
            options.AddPolicy("per-client", httpContext =>
            {
                var clientId = httpContext.Request.Headers["X-Client-Id"].FirstOrDefault()
                    ?? httpContext.Connection.RemoteIpAddress?.ToString()
                    ?? "anonymous";

                return RateLimitPartition.GetFixedWindowLimiter(clientId, _ => new FixedWindowRateLimiterOptions
                {
                    Window = TimeSpan.FromSeconds(60),
                    PermitLimit = 50,
                    QueueLimit = 5,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
            });
        });

        return services;
    }
}
