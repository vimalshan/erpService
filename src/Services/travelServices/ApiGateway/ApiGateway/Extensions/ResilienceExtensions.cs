using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace ApiGateway.Extensions;

public static class ResilienceExtensions
{
    public static IServiceCollection AddGatewayResilience(this IServiceCollection services)
    {
        // Register a named HttpClient with Polly policies for each service
        var serviceEndpoints = new Dictionary<string, string>
        {
            ["TravelRequestService"] = "http://localhost:5205",
            ["TravelTransactionService"] = "http://localhost:5082",
            ["BookingService"] = "http://localhost:5117",
            ["ExpenseService"] = "http://localhost:5090",
            ["FinanceService"] = "http://localhost:5294",
            ["InsuranceService"] = "http://localhost:5179",
            ["MasterDataService"] = "http://localhost:5166",
            ["AgencyService"] = "http://localhost:5000",
            ["AdminService"] = "http://localhost:5001"
        };

        foreach (var (name, baseAddress) in serviceEndpoints)
        {
            services.AddHttpClient(name, client =>
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Add("X-Forwarded-Gateway", "ApiGateway");
            })
            // Retry policy: 3 retries with exponential backoff
            .AddPolicyHandler(GetRetryPolicy())
            // Circuit breaker: open after 5 failures, stay open 30s
            .AddPolicyHandler(GetCircuitBreakerPolicy())
            // Timeout: 30 seconds per request
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30)));
        }

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 1000)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    // Polly context logging
                });
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, breakDelay) => { },
                onReset: () => { },
                onHalfOpen: () => { });
    }
}
