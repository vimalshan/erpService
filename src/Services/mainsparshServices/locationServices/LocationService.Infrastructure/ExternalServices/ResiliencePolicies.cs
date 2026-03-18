using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace LocationService.Infrastructure.ExternalServices
{
    /// <summary>
    /// Polly resilience policies for HTTP calls
    /// </summary>
    public static class ResiliencePolicies
    {
        /// <summary>
        /// Circuit Breaker Policy - prevents cascading failures
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync<HttpResponseMessage>(
                    handledEventsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (outcome, timeSpan) =>
                    {
                        Console.WriteLine($"Circuit breaker opened for 30 seconds");
                    },
                    onReset: () =>
                    {
                        Console.WriteLine("Circuit breaker reset");
                    });
        }

        /// <summary>
        /// Retry Policy with exponential backoff
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync<HttpResponseMessage>(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        Console.WriteLine($"Retry {retryCount} after {timespan.Milliseconds}ms");
                    });
        }

        /// <summary>
        /// Timeout Policy
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Combined policy: Timeout -> Retry -> CircuitBreaker
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
        {
            var timeoutPolicy = GetTimeoutPolicy();
            var retryPolicy = GetRetryPolicy();
            var circuitBreakerPolicy = GetCircuitBreakerPolicy();

            return Policy.WrapAsync(timeoutPolicy, retryPolicy, circuitBreakerPolicy);
        }
    }
}
