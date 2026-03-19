using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.CircuitBreaker;

namespace SecurityService.API.Resilience;

/// <summary>
/// Polly-based resilience configuration for outbound HTTP clients and internal policies.
/// </summary>
public static class ResiliencePolicies
{
    /// <summary>Standard circuit breaker pipeline name registered in DI.</summary>
    public const string DefaultPipelineName = "default-circuit-breaker";

    /// <summary>
    /// Configures resilience pipelines on the IServiceCollection and adds
    /// a typed circuit-breaker-wrapped HttpClient.
    /// </summary>
    public static IServiceCollection AddResiliencePolicies(this IServiceCollection services)
    {
        // Resilience pipeline: retry + circuit breaker for outbound HTTP calls
        services.AddResiliencePipeline(DefaultPipelineName, piplineBuilder =>
        {
            piplineBuilder
                .AddRetry(new Polly.Retry.RetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromMilliseconds(500),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    ShouldHandle = new PredicateBuilder().Handle<Exception>()
                })
                .AddCircuitBreaker(new CircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    MinimumThroughput = 5,
                    BreakDuration = TimeSpan.FromSeconds(30),
                    ShouldHandle = new PredicateBuilder().Handle<Exception>()
                })
                .AddTimeout(TimeSpan.FromSeconds(10));
        });

        // Example: resilient named HttpClient for external calls
        services.AddHttpClient("resilient-client")
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.MaxRetryAttempts = 3;
                options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(15);
                options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(30);
            });

        return services;
    }
}
