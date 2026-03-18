using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace ObjectiveService.Infrastructure.Services;

public static class PollyPolicies
{
    /// <summary>
    /// Retry up to 3 times with exponential back-off (2 s, 4 s, 8 s).
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (outcome, timespan, retry, _) =>
                {
                    Console.WriteLine($"[Polly] Retry {retry} after {timespan.TotalSeconds:0.##}s — {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                });

    /// <summary>
    /// Circuit breaker: open after 5 consecutive failures, reset after 30 s.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, duration) =>
                    Console.WriteLine($"[Polly] Circuit OPEN for {duration.TotalSeconds}s — {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}"),
                onReset: () => Console.WriteLine("[Polly] Circuit CLOSED"),
                onHalfOpen: () => Console.WriteLine("[Polly] Circuit HALF-OPEN"));

    /// <summary>
    /// Timeout policy — 10-second absolute timeout per request.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy() =>
        Policy.TimeoutAsync<HttpResponseMessage>(seconds: 10);
}
