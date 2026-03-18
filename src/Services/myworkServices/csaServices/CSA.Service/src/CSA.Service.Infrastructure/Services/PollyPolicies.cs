using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace CSA.Service.Infrastructure.Services;

public static class PollyPolicies
{
    public static IServiceCollection AddCircuitBreakerPolicies(this IServiceCollection services)
    {
        // Named HttpClient with Polly retry + circuit breaker
        services.AddHttpClient("CsaExternalApi", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .AddResilienceHandler("csa-pipeline", builder =>
        {
            // Retry policy: 3 retries with exponential backoff
            builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential,
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<HttpRequestException>()
                    .HandleResult(r => !r.IsSuccessStatusCode)
            });

            // Circuit breaker: break after 5 failures, stay open 30s
            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>
            {
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 5,
                BreakDuration = TimeSpan.FromSeconds(30),
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<HttpRequestException>()
                    .HandleResult(r => !r.IsSuccessStatusCode)
            });

            // Timeout per attempt
            builder.AddTimeout(TimeSpan.FromSeconds(10));
        });

        return services;
    }
}
