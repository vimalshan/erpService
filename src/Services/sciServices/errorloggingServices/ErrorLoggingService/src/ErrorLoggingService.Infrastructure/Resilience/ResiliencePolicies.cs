using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace ErrorLoggingService.Infrastructure.Resilience;

public static class ResiliencePolicies
{
    public static AsyncRetryPolicy GetRetryPolicy(int retryCount = 3)
        => Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount,
                attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                (exception, timeSpan, attempt, _) =>
                {
                    Console.WriteLine($"[Retry {attempt}] Waiting {timeSpan} before next attempt. Exception: {exception.Message}");
                });

    public static AsyncCircuitBreakerPolicy GetCircuitBreakerPolicy(
        int exceptionsAllowedBeforeBreaking = 5,
        int durationOfBreakSeconds = 30)
        => Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking,
                TimeSpan.FromSeconds(durationOfBreakSeconds),
                onBreak: (ex, breakDelay) =>
                    Console.WriteLine($"[Circuit OPEN] Breaking for {breakDelay.TotalSeconds}s. Error: {ex.Message}"),
                onReset: () =>
                    Console.WriteLine("[Circuit CLOSED] Circuit reset."),
                onHalfOpen: () =>
                    Console.WriteLine("[Circuit HALF-OPEN] Testing next call."));

    public static IAsyncPolicy GetCombinedPolicy() =>
        Policy.WrapAsync(GetRetryPolicy(), GetCircuitBreakerPolicy());
}
