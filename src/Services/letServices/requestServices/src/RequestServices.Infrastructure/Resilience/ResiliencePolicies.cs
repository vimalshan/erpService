using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace RequestServices.Infrastructure.Resilience;

/// <summary>Polly resilience pipeline definitions for outbound HTTP and database calls.</summary>
public static class ResiliencePolicies
{
    /// <summary>
    /// Returns a ResiliencePipeline with retry + circuit-breaker for SQL/RabbitMQ operations.
    /// </summary>
    public static ResiliencePipeline<T> CreateDbPipeline<T>(IServiceProvider sp) =>
        new ResiliencePipelineBuilder<T>()
            .AddRetry(new RetryStrategyOptions<T>
            {
                ShouldHandle   = new PredicateBuilder<T>().Handle<Exception>(),
                MaxRetryAttempts = 3,
                Delay          = TimeSpan.FromMilliseconds(500),
                BackoffType    = DelayBackoffType.Exponential
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions<T>
            {
                ShouldHandle        = new PredicateBuilder<T>().Handle<Exception>(),
                FailureRatio        = 0.5,
                SamplingDuration    = TimeSpan.FromSeconds(10),
                MinimumThroughput   = 5,
                BreakDuration       = TimeSpan.FromSeconds(30)
            })
            .Build();
}
