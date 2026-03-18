namespace CommunityService.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Timeout;

public interface IPolicyRegistry
{
    IAsyncPolicy<HttpResponseMessage> GetHttpClientPolicy();
    IAsyncPolicy GetDatabasePolicy();
}

public class PolicyRegistry : IPolicyRegistry
{
    private readonly IAsyncPolicy<HttpResponseMessage> _httpClientPolicy;
    private readonly IAsyncPolicy _databasePolicy;

    public PolicyRegistry(IConfiguration configuration)
    {
        var circuitBreakerSettings = configuration.GetSection("CircuitBreaker");
        var timeoutDuration = int.Parse(circuitBreakerSettings["TimeoutDuration"] ?? "5");

        // Simple timeout policy for HTTP
        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(timeoutDuration));

        // Retry policy for transient failures
        var retryPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .OrResult(r => (int)r.StatusCode >= 500)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

        // Compose all policies
        _httpClientPolicy = Policy.WrapAsync<HttpResponseMessage>(retryPolicy, timeoutPolicy);

        // Database retry policy
        _databasePolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (outcome, timespan, retryNumber, context) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Database retry {retryNumber} after {timespan.TotalSeconds} seconds");
                });
    }

    public IAsyncPolicy<HttpResponseMessage> GetHttpClientPolicy() => _httpClientPolicy;

    public IAsyncPolicy GetDatabasePolicy() => _databasePolicy;
}
