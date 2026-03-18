using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace TrainingDevelopment.API.Resilience;

public static class ResiliencePolicies
{
    public static ResiliencePipeline CreateDbResiliencePipeline(IConfiguration configuration)
    {
        var retryCount = int.Parse(configuration["Polly:RetryCount"] ?? "3");
        var cbThreshold = int.Parse(configuration["Polly:CircuitBreakerThreshold"] ?? "5");
        var cbDuration = int.Parse(configuration["Polly:CircuitBreakerDurationSeconds"] ?? "30");

        return new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = retryCount,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                OnRetry = args =>
                {
                    Console.WriteLine($"Retry attempt {args.AttemptNumber}: {args.Outcome.Exception?.Message}");
                    return ValueTask.CompletedTask;
                }
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                MinimumThroughput = cbThreshold,
                BreakDuration = TimeSpan.FromSeconds(cbDuration),
                OnOpened = args =>
                {
                    Console.WriteLine($"Circuit breaker opened: {args.Outcome.Exception?.Message}");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }
}
