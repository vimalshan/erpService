using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace VendorService.Infrastructure.Resilience;

public static class ResiliencePolicies
{
    public static ResiliencePipeline DatabaseRetry() =>
        new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential,
                ShouldHandle = new PredicateBuilder()
                    .Handle<Microsoft.Data.SqlClient.SqlException>()
                    .Handle<TimeoutException>()
            })
            .Build();

    public static ResiliencePipeline DatabaseCircuitBreaker() =>
        new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                MinimumThroughput = 5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = TimeSpan.FromSeconds(15),
                ShouldHandle = new PredicateBuilder()
                    .Handle<Microsoft.Data.SqlClient.SqlException>()
                    .Handle<TimeoutException>()
            })
            .Build();

    public static ResiliencePipeline BlobStorageRetry() =>
        new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                BackoffType = DelayBackoffType.Exponential,
                ShouldHandle = new PredicateBuilder()
                    .Handle<Azure.RequestFailedException>()
            })
            .Build();
}
