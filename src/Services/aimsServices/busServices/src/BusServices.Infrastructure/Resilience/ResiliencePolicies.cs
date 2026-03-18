using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace BusServices.Infrastructure.Resilience;

public static class ResiliencePolicies
{
    public static ResiliencePipeline<TResult> CreateDatabasePipeline<TResult>()
    {
        return new ResiliencePipelineBuilder<TResult>()
            .AddRetry(new RetryStrategyOptions<TResult>
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                ShouldHandle = new PredicateBuilder<TResult>()
                    .Handle<Exception>()
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions<TResult>
            {
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 5,
                BreakDuration = TimeSpan.FromSeconds(15)
            })
            .Build();
    }

    public static ResiliencePipeline<TResult> CreateHttpPipeline<TResult>()
    {
        return new ResiliencePipelineBuilder<TResult>()
            .AddRetry(new RetryStrategyOptions<TResult>
            {
                MaxRetryAttempts = 2,
                Delay = TimeSpan.FromMilliseconds(500),
                BackoffType = DelayBackoffType.Linear
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions<TResult>
            {
                FailureRatio = 0.6,
                SamplingDuration = TimeSpan.FromSeconds(60),
                MinimumThroughput = 3,
                BreakDuration = TimeSpan.FromSeconds(30)
            })
            .Build();
    }
}
