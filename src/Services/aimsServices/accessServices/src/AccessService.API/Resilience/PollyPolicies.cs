using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace AccessService.API.Resilience
{
    /// <summary>
    /// Polly resilience policies for external service calls
    /// Implements retry, circuit breaker, and timeout policies
    /// </summary>
    public static class PollyPolicies
    {
        // Retry policy: 3 attempts with exponential backoff
        private static readonly IAsyncPolicy<HttpResponseMessage> RetryPolicy =
            Policy
                .Handle<HttpRequestException>()
                .Or<TimeoutRejectedException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode && (int)r.StatusCode >= 500)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(Math.Pow(2, attempt) * 100),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        // Logging can be added here
                    });

        // Circuit breaker policy: Opens after 5 failures, half-open after 30 seconds
        private static readonly IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy =
            Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode && (int)r.StatusCode >= 500)
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (outcome, timespan) =>
                    {
                        // Logging can be added here
                    },
                    onReset: () =>
                    {
                        // Logging can be added here
                    });

        // Timeout policy: 10 seconds per request
        private static readonly IAsyncPolicy<HttpResponseMessage> TimeoutPolicy =
            Policy
                .TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));

        // Combined policy: Timeout -> Retry -> Circuit Breaker
        public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
        {
            return Policy.WrapAsync(TimeoutPolicy, RetryPolicy, CircuitBreakerPolicy);
        }

        // Individual policies
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() => RetryPolicy;
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() => CircuitBreakerPolicy;
        public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy() => TimeoutPolicy;
    }
}
