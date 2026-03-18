using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.CircuitBreaker;

namespace TimesheetService.Infrastructure.Resilience;

public static class ResiliencePolicies
{
    public static IHttpClientBuilder AddTimesheetResilienceHandler(this IHttpClientBuilder builder)
    {
        builder.AddResilienceHandler("timesheet-pipeline", pipeline =>
        {
            // Retry: 3 attempts with exponential backoff
            pipeline.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay            = TimeSpan.FromSeconds(1),
                BackoffType      = DelayBackoffType.Exponential,
                UseJitter        = true
            });

            // Circuit Breaker: open after 5 failures in 30 seconds
            pipeline.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                SamplingDuration       = TimeSpan.FromSeconds(30),
                FailureRatio           = 0.5,
                MinimumThroughput      = 5,
                BreakDuration          = TimeSpan.FromSeconds(15)
            });

            // Timeout: 10 seconds per attempt
            pipeline.AddTimeout(TimeSpan.FromSeconds(10));
        });
        return builder;
    }
}
