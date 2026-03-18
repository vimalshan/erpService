using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;

namespace MemberService.API.Resilience;

public static class ResiliencePolicies
{
    /// <summary>
    /// Retry policy: 3 retries with exponential back-off (2s, 4s, 8s) on transient HTTP errors.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

    /// <summary>
    /// Circuit Breaker: opens after 5 consecutive failures for 30 seconds.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30));

    /// <summary>
    /// Combined policy: retry + circuit breaker wrapping.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy() =>
        Policy.WrapAsync(GetRetryPolicy(), GetCircuitBreakerPolicy());
}
