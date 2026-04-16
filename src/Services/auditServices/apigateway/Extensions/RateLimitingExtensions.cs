using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiGateway.Extensions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddGatewayRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var globalSection  = configuration.GetSection("RateLimiting:Global");
        var perUserSection = configuration.GetSection("RateLimiting:PerUser");

        int globalPermit  = globalSection.GetValue<int>("PermitLimit", 1000);
        int globalWindow  = globalSection.GetValue<int>("WindowSeconds", 60);
        int globalQueue   = globalSection.GetValue<int>("QueueLimit", 20);

        int userPermit    = perUserSection.GetValue<int>("PermitLimit", 200);
        int userWindow    = perUserSection.GetValue<int>("WindowSeconds", 60);
        int userQueue     = perUserSection.GetValue<int>("QueueLimit", 5);

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (ctx, ct) =>
            {
                var logger = ctx.HttpContext.RequestServices
                    .GetRequiredService<ILogger<RateLimiterOptions>>();
                logger.LogWarning(
                    "Rate limit exceeded | CorrelationId: {CorrelationId} | IP: {IP} | Path: {Path}",
                    ctx.HttpContext.Items["CorrelationId"],
                    ctx.HttpContext.Connection.RemoteIpAddress,
                    ctx.HttpContext.Request.Path);

                ctx.HttpContext.Response.ContentType = "application/json";
                await ctx.HttpContext.Response.WriteAsJsonAsync(new
                {
                    error = "Too many requests",
                    message = "Rate limit exceeded. Please retry after a moment.",
                    retryAfter = ctx.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                        ? (int)retryAfter.TotalSeconds
                        : globalWindow
                }, ct);
            };

            // Global sliding window — applies to all routes
            options.AddSlidingWindowLimiter("global", opt =>
            {
                opt.PermitLimit       = globalPermit;
                opt.Window            = TimeSpan.FromSeconds(globalWindow);
                opt.SegmentsPerWindow = 4;
                opt.QueueLimit        = globalQueue;
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            // Per-user sliding window — keyed by JWT subject or IP
            options.AddPolicy("per-user", ctx =>
            {
                var userId = ctx.User?.Identity?.Name
                             ?? ctx.Connection.RemoteIpAddress?.ToString()
                             ?? "anonymous";

                return RateLimitPartition.GetSlidingWindowLimiter(userId, _ =>
                    new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit          = userPermit,
                        Window               = TimeSpan.FromSeconds(userWindow),
                        SegmentsPerWindow    = 4,
                        QueueLimit           = userQueue,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });
            });
        });

        return services;
    }
}
