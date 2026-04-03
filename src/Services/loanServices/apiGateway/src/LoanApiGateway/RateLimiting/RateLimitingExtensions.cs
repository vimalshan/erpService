using System.Threading.RateLimiting;

namespace LoanApiGateway.RateLimiting;

/// <summary>
/// Configures ASP.NET Core's built-in rate limiting with:
///  - Fixed Window  for anonymous callers (IP-based)
///  - Token Bucket  for authenticated callers (user-based)
///  - Concurrency   for heavy mutation endpoints (/loans/disburse, /transactions)
///  - Sliding Window for GraphQL queries
/// </summary>
public static class RateLimitingExtensions
{
    public const string AnonymousPolicy = "anonymous";
    public const string AuthenticatedPolicy = "authenticated";
    public const string MutationPolicy = "mutations";
    public const string GraphQlPolicy = "graphql";

    public static IServiceCollection AddGatewayRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var cfg = configuration.GetSection("Gateway:RateLimiting");

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (ctx, _) =>
            {
                ctx.HttpContext.Response.Headers["Retry-After"] = "60";
                await ctx.HttpContext.Response.WriteAsJsonAsync(new
                {
                    error = "Rate limit exceeded. Please slow down.",
                    retryAfter = 60
                });
            };

            // --- Anonymous callers: Fixed Window 60 req / 60 sec per IP ---
            options.AddPolicy(AnonymousPolicy, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = cfg.GetValue("Anonymous:PermitLimit", 60),
                        Window = TimeSpan.FromSeconds(cfg.GetValue("Anonymous:WindowSeconds", 60)),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 5
                    }));

            // --- Authenticated callers: Token Bucket 200 tokens refill 10/sec ---
            options.AddPolicy(AuthenticatedPolicy, httpContext =>
            {
                var userId = httpContext.User.FindFirst("sub")?.Value
                             ?? httpContext.User.Identity?.Name
                             ?? httpContext.Connection.RemoteIpAddress?.ToString()
                             ?? "unknown";

                return RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: userId,
                    factory: _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = cfg.GetValue("Authenticated:TokenLimit", 200),
                        TokensPerPeriod = cfg.GetValue("Authenticated:TokensPerPeriod", 10),
                        ReplenishmentPeriod = TimeSpan.FromSeconds(cfg.GetValue("Authenticated:ReplenishmentSeconds", 1)),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 10,
                        AutoReplenishment = true
                    });
            });

            // --- Mutations (disburse, transaction writes): Concurrency limit ---
            options.AddPolicy(MutationPolicy, httpContext =>
                RateLimitPartition.GetConcurrencyLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new ConcurrencyLimiterOptions
                    {
                        PermitLimit = cfg.GetValue("Mutations:MaxConcurrent", 10),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 5
                    }));

            // --- GraphQL: Sliding Window 30 req / 30 sec per IP ---
            options.AddPolicy(GraphQlPolicy, httpContext =>
                RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = cfg.GetValue("GraphQL:PermitLimit", 30),
                        Window = TimeSpan.FromSeconds(cfg.GetValue("GraphQL:WindowSeconds", 30)),
                        SegmentsPerWindow = 3,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 5
                    }));
        });

        return services;
    }
}
