using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace TransactionService.Shared.Utilities;

public interface IResiliencePolicy
{
    IAsyncPolicy<HttpResponseMessage> GetHttpPolicy();
}

public class ResiliencePolicyService : IResiliencePolicy
{
    public IAsyncPolicy<HttpResponseMessage> GetHttpPolicy()
    {
        // Circuit Breaker Policy
        var circuitBreakerPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, timespan, context) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Circuit breaker opened for {timespan.TotalSeconds} seconds");
                },
                onReset: (context) =>
                {
                    System.Diagnostics.Debug.WriteLine("Circuit breaker reset");
                }
            );

        // Retry Policy
        var retryPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(r => (int)r.StatusCode >= 500 || r.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Retry attempt {retryCount} after {timespan.TotalSeconds} seconds");
                }
            );

        // Timeout Policy
        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(
            TimeSpan.FromSeconds(10));

        // Combine all policies
        return Policy.WrapAsync(circuitBreakerPolicy, retryPolicy, timeoutPolicy);
    }
}
