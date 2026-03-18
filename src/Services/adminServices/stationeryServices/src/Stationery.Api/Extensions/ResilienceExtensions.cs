using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.CircuitBreaker;

namespace Stationery.Api.Extensions;

public static class ResilienceExtensions
{
    public static IServiceCollection AddResiliencePolicies(this IServiceCollection services)
    {
        // 1. Retry Policy
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        // 2. Circuit Breaker Policy
        var circuitBreakerPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

        // Registry
        services.AddSingleton<AsyncRetryPolicy<HttpResponseMessage>>(retryPolicy);
        services.AddSingleton<AsyncCircuitBreakerPolicy<HttpResponseMessage>>(circuitBreakerPolicy);

        return services;
    }
}
