using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using System.Net;
using System.Threading.RateLimiting;

namespace ApiGateway.Resilience;

public static class ResilienceExtensions
{
    public static IServiceCollection AddGatewayResilience(this IServiceCollection services)
    {
        services.AddResiliencePipeline("gateway-pipeline", builder =>
        {
            // ── Bulkhead Isolation ──────────────────────────────
            builder.AddConcurrencyLimiter(new ConcurrencyLimiterOptions
            {
                PermitLimit = 100,
                QueueLimit = 50
            });

            // ── Retry ───────────────────────────────────────────
            builder.AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>()
                    .Handle<TimeoutRejectedException>(),
                OnRetry = args =>
                {
                    var logger = args.Context.Properties.GetValue(
                        new ResiliencePropertyKey<ILogger>("Logger"), null!);
                    logger?.LogWarning(
                        "Retry attempt {Attempt} after {Delay}ms for {OperationKey}",
                        args.AttemptNumber, args.RetryDelay.TotalMilliseconds,
                        args.Context.OperationKey);
                    return ValueTask.CompletedTask;
                }
            });

            // ── Circuit Breaker ─────────────────────────────────
            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 10,
                BreakDuration = TimeSpan.FromSeconds(15),
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>()
                    .Handle<TimeoutRejectedException>(),
                OnOpened = args =>
                {
                    var logger = args.Context.Properties.GetValue(
                        new ResiliencePropertyKey<ILogger>("Logger"), null!);
                    logger?.LogWarning("Circuit OPENED for {OperationKey} — break {BreakDuration}s",
                        args.Context.OperationKey, args.BreakDuration.TotalSeconds);
                    return ValueTask.CompletedTask;
                },
                OnClosed = args =>
                {
                    var logger = args.Context.Properties.GetValue(
                        new ResiliencePropertyKey<ILogger>("Logger"), null!);
                    logger?.LogInformation("Circuit CLOSED for {OperationKey}", args.Context.OperationKey);
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = args =>
                {
                    var logger = args.Context.Properties.GetValue(
                        new ResiliencePropertyKey<ILogger>("Logger"), null!);
                    logger?.LogInformation("Circuit HALF-OPEN for {OperationKey}", args.Context.OperationKey);
                    return ValueTask.CompletedTask;
                }
            });

            // ── Timeout ─────────────────────────────────────────
            builder.AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromSeconds(30),
                OnTimeout = args =>
                {
                    var logger = args.Context.Properties.GetValue(
                        new ResiliencePropertyKey<ILogger>("Logger"), null!);
                    logger?.LogWarning("Request timeout after {Timeout}s for {OperationKey}",
                        args.Timeout.TotalSeconds, args.Context.OperationKey);
                    return ValueTask.CompletedTask;
                }
            });
        });

        // ── Per-cluster pipelines (for finer control) ───────────────
        var clusterNames = new[]
        {
            "leave-cluster", "course-cluster", "request-cluster",
            "review-cluster", "development-cluster", "master-cluster",
            "let-transaction-cluster"
        };

        foreach (var cluster in clusterNames)
        {
            services.AddResiliencePipeline(cluster, builder =>
            {
                builder.AddConcurrencyLimiter(new ConcurrencyLimiterOptions
                {
                    PermitLimit = 20,
                    QueueLimit = 10
                });

                builder.AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = 2,
                    Delay = TimeSpan.FromMilliseconds(300),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    ShouldHandle = new PredicateBuilder()
                        .Handle<HttpRequestException>()
                        .Handle<TimeoutRejectedException>()
                });

                builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    MinimumThroughput = 5,
                    BreakDuration = TimeSpan.FromSeconds(10),
                    ShouldHandle = new PredicateBuilder()
                        .Handle<HttpRequestException>()
                        .Handle<TimeoutRejectedException>()
                });

                builder.AddTimeout(TimeSpan.FromSeconds(15));
            });
        }

        return services;
    }
}
