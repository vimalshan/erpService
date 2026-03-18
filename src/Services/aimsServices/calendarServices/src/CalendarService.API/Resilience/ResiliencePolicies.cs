using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace CalendarService.API.Resilience;

public static class ResiliencePolicies
{
    public static IAsyncPolicy<HttpResponseMessage> GetHttpCircuitBreakerPolicy(ILogger logger)
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (_, duration) => logger.LogWarning("Circuit breaker OPEN for {Duration}", duration),
                onReset: () => logger.LogInformation("Circuit breaker RESET"),
                onHalfOpen: () => logger.LogInformation("Circuit breaker HALF-OPEN"));
    }

    public static IAsyncPolicy<HttpResponseMessage> GetHttpRetryPolicy(ILogger logger) =>
        Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                (result, delay, attempt, _) =>
                    logger.LogWarning("Retry {Attempt} after {Delay}ms", attempt, delay.TotalMilliseconds));

    public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy(ILogger logger)
        => Policy.WrapAsync(GetHttpCircuitBreakerPolicy(logger), GetHttpRetryPolicy(logger));
}
