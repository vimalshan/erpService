using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace InvoiceProcessing.Infrastructure.Resilience;

public static class PollyPolicies
{
    public static AsyncRetryPolicy CreateRetryPolicy(int retryCount = 3)
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(retryCount, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryAttempt, _) =>
                {
                    // Logging handled by caller
                });
    }

    public static AsyncCircuitBreakerPolicy CreateCircuitBreakerPolicy(
        int exceptionsAllowedBeforeBreaking = 5,
        int durationOfBreakInSeconds = 30)
    {
        return Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking,
                TimeSpan.FromSeconds(durationOfBreakInSeconds));
    }

    public static IAsyncPolicy CreateCombinedPolicy()
    {
        var retryPolicy = CreateRetryPolicy();
        var circuitBreakerPolicy = CreateCircuitBreakerPolicy();

        return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
    }
}
