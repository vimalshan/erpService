using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace CurrencyManagement.Infrastructure.Resilience;

/// <summary>
/// Registry for Polly resilience policies
/// </summary>
public class PolicyRegistry
{
    /// <summary>
    /// Gets a retry policy for transient failures
    /// </summary>
    public static AsyncPolicy<HttpResponseMessage> GetHttpRetryPolicy()
    {
        return Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .Or<HttpRequestException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} after {timeSpan.TotalSeconds}s");
                });
    }

    /// <summary>
    /// Gets a circuit breaker policy for failing services
    /// </summary>
    public static AsyncPolicy<HttpResponseMessage> GetHttpCircuitBreakerPolicy()
    {
        return Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .Or<HttpRequestException>()
            .CircuitBreakerAsync<HttpResponseMessage>(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30));
    }

    /// <summary>
    /// Gets a combined retry + circuit breaker policy
    /// </summary>
    public static AsyncPolicy<HttpResponseMessage> GetHttpMultiPolicy()
    {
        var retryPolicy = GetHttpRetryPolicy();
        var circuitBreakerPolicy = GetHttpCircuitBreakerPolicy();
        return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
    }
}
