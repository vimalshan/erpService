using Polly;

namespace PayrollServices.Infrastructure.Services;

/// <summary>
/// Polly policy provider for circuit breaker and resilience patterns
/// </summary>
public class PolicyProvider
{
    public static IAsyncPolicy<T> GetRetryPolicy<T>() where T : class
    {
        return Policy<T>
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .OrResult(r => r == null)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(Math.Pow(2, attempt) * 100),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    Console.WriteLine($"Retry attempt {retryAttempt} after {timespan.TotalMilliseconds}ms");
                });
    }

    public static IAsyncPolicy<T> GetCombinedPolicy<T>() where T : class
    {
        return Policy.WrapAsync(GetRetryPolicy<T>());
    }

    public static IAsyncPolicy GetRetryPolicyForVoid()
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(Math.Pow(2, attempt) * 100),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    Console.WriteLine($"Retry attempt {retryAttempt} after {timespan.TotalMilliseconds}ms");
                });
    }

    /// <summary>
    /// Gets a timeout policy for async operations
    /// </summary>
    public static IAsyncPolicy<T> GetTimeoutPolicy<T>(TimeSpan timeout) where T : class
    {
        return Policy.TimeoutAsync<T>(timeout);
    }
}
