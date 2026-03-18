using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;

namespace LoanDefinition.Infrastructure.Resilience;

public static class PollyPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }

    public static AsyncRetryPolicy GetDatabaseRetryPolicy(ILogger logger)
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryCount, _) =>
                {
                    logger.LogWarning(exception, "Database retry {RetryCount} after {Delay}s", retryCount, timeSpan.TotalSeconds);
                });
    }

    public static AsyncCircuitBreakerPolicy GetDatabaseCircuitBreakerPolicy(ILogger logger)
    {
        return Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(60),
                onBreak: (exception, duration) =>
                    logger.LogError(exception, "Circuit breaker opened for {Duration}s", duration.TotalSeconds),
                onReset: () =>
                    logger.LogInformation("Circuit breaker reset"),
                onHalfOpen: () =>
                    logger.LogInformation("Circuit breaker half-open"));
    }

    public static IServiceCollection AddPollyPolicies(this IServiceCollection services)
    {
        services.AddHttpClient("ExternalService")
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        return services;
    }
}
