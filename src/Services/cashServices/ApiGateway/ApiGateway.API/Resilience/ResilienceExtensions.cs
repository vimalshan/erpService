using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace ApiGateway.API.Resilience;

public static class ResilienceExtensions
{
    public static IServiceCollection AddGatewayResilience(this IServiceCollection services)
    {
        // Named HttpClient for health checks with resilience pipeline
        services.AddHttpClient("HealthCheck")
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.MaxRetryAttempts = 2;
                options.Retry.Delay = TimeSpan.FromMilliseconds(500);
                options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
                options.CircuitBreaker.FailureRatio = 0.5;
                options.CircuitBreaker.MinimumThroughput = 5;
                options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(15);
                options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(5);
                options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(30);
            });

        // Named HttpClient for downstream service calls with resilience
        services.AddHttpClient("Downstream")
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.MaxRetryAttempts = 3;
                options.Retry.Delay = TimeSpan.FromSeconds(1);
                options.Retry.BackoffType = DelayBackoffType.Exponential;
                options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(60);
                options.CircuitBreaker.FailureRatio = 0.3;
                options.CircuitBreaker.MinimumThroughput = 10;
                options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(30);
                options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
                options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(60);
            });

        return services;
    }
}
