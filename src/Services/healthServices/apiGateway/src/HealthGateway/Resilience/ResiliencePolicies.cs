using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;

namespace HealthGateway.Resilience;

public static class ResiliencePolicies
{
    /// <summary>Retry 3 times with exponential backoff (1s, 2s, 4s).</summary>
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)),
                onRetry: (outcome, timespan, attempt, _) =>
                {
                    var logger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger("RetryPolicy");
                    logger.LogWarning("Retry {Attempt} after {Delay}s due to: {Reason}",
                        attempt, timespan.TotalSeconds, outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString());
                });

    /// <summary>Timeout after 30 seconds per request.</summary>
    public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy() =>
        Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));

    /// <summary>Break circuit after 5 consecutive failures; open for 30 seconds.</summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, duration) =>
                {
                    var logger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger("CircuitBreaker");
                    logger.LogError("Circuit OPEN for {Duration}s. Reason: {Reason}",
                        duration.TotalSeconds, outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString());
                },
                onReset: () =>
                {
                    var logger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger("CircuitBreaker");
                    logger.LogInformation("Circuit CLOSED — service recovered.");
                },
                onHalfOpen: () =>
                {
                    var logger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger("CircuitBreaker");
                    logger.LogInformation("Circuit HALF-OPEN — probing service.");
                });

    /// <summary>Combined policy: Timeout → CircuitBreaker → Retry (outermost first).</summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy() =>
        Policy.WrapAsync(GetRetryPolicy(), GetCircuitBreakerPolicy(), GetTimeoutPolicy());
}
