using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Registry;
using Polly.Retry;

namespace GSTComplianceService.Infrastructure.Resilience;

public static class ResiliencePolicies
{
    public const string DefaultRetryPolicy = "DefaultRetry";
    public const string DatabaseCircuitBreaker = "DatabaseCircuitBreaker";
    public const string BlobStorageCircuitBreaker = "BlobStorageCircuitBreaker";

    public static IServiceCollection AddResiliencePolicies(this IServiceCollection services)
    {
        services.AddResiliencePipeline(DefaultRetryPolicy, builder =>
        {
            builder.AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(300),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                ShouldHandle = new PredicateBuilder().Handle<Exception>(ex => ex is not OperationCanceledException)
            });
        });

        services.AddResiliencePipeline(DatabaseCircuitBreaker, builder =>
        {
            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                SamplingDuration = TimeSpan.FromSeconds(30),
                FailureRatio = 0.5,
                MinimumThroughput = 5,
                BreakDuration = TimeSpan.FromSeconds(15),
                OnOpened = args =>
                {
                    // Log circuit opened
                    return ValueTask.CompletedTask;
                },
                OnClosed = args =>
                {
                    return ValueTask.CompletedTask;
                }
            });
        });

        services.AddResiliencePipeline(BlobStorageCircuitBreaker, builder =>
        {
            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                SamplingDuration = TimeSpan.FromSeconds(60),
                FailureRatio = 0.6,
                MinimumThroughput = 3,
                BreakDuration = TimeSpan.FromSeconds(30)
            });

            builder.AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 2,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Linear
            });
        });

        return services;
    }
}
