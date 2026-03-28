using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace ApiGateway.Extensions;

public static class ResilienceExtensions
{
    public static IServiceCollection AddResilienceServices(this IServiceCollection services)
    {
        // Named HttpClient with Polly policies for downstream calls
        services.AddHttpClient("DownstreamClient", client =>
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        // ─── Retry Policy ─────────────────────────────────────────────────────
        .AddPolicyHandler(GetRetryPolicy())
        // ─── Circuit Breaker ──────────────────────────────────────────────────
        .AddPolicyHandler(GetCircuitBreakerPolicy())
        // ─── Timeout ──────────────────────────────────────────────────────────
        .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)));

        // ─── Bulkhead Isolation ─────────────────────────────────────────────────
        services.AddSingleton<BulkheadRegistry>();

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    var logger = context.TryGetValue("logger", out var loggerObj)
                        ? loggerObj as ILogger : null;
                    logger?.LogWarning(
                        "Retry {RetryAttempt} after {Delay}s for {PolicyKey}. Status: {StatusCode}",
                        retryAttempt, timespan.TotalSeconds,
                        context.PolicyKey,
                        outcome.Result?.StatusCode);
                });
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, breakDelay) =>
                {
                    // Circuit opened
                },
                onReset: () =>
                {
                    // Circuit closed
                },
                onHalfOpen: () =>
                {
                    // Circuit half-open, testing
                });
    }
}

/// <summary>
/// Registry for bulkhead policies per service — limits concurrent requests to each downstream service.
/// </summary>
public class BulkheadRegistry
{
    private readonly Dictionary<string, Polly.Bulkhead.AsyncBulkheadPolicy> _bulkheads = new();
    private readonly object _lock = new();

    public Polly.Bulkhead.AsyncBulkheadPolicy GetBulkhead(string serviceName, int maxParallelization = 25, int maxQueuingActions = 10)
    {
        lock (_lock)
        {
            if (!_bulkheads.TryGetValue(serviceName, out var policy))
            {
                policy = Policy.BulkheadAsync(maxParallelization, maxQueuingActions,
                    onBulkheadRejectedAsync: context =>
                    {
                        throw new BulkheadRejectedException($"Service '{serviceName}' is at capacity. Please try again later.");
                    });
                _bulkheads[serviceName] = policy;
            }
            return policy;
        }
    }
}

public class BulkheadRejectedException : Exception
{
    public BulkheadRejectedException(string message) : base(message) { }
}
