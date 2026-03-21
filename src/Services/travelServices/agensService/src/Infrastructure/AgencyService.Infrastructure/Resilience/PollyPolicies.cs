using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Microsoft.Extensions.DependencyInjection;

namespace AgencyService.Infrastructure.Resilience;

public static class PollyPoliciesExtensions
{
    public static IServiceCollection AddPollyPolicies(this IServiceCollection services)
    {
        // Retry policy - handles transient failures
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => (int)r.StatusCode >= 500)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (outcome, timespan, attempts, context) =>
                {
                    Console.WriteLine($"Retrying request. Attempt {attempts} after {timespan.TotalSeconds} seconds");
                });
        
        // Circuit breaker policy - prevents cascading failures
        var circuitBreakerPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => (int)r.StatusCode >= 500)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, timespan) =>
                {
                    Console.WriteLine($"Circuit breaker opened for {timespan.TotalSeconds} seconds");
                },
                onReset: () =>
                {
                    Console.WriteLine("Circuit breaker reset");
                });
        
        // Register policies as scoped services
        services.AddScoped(sp => retryPolicy);
        services.AddScoped(sp => circuitBreakerPolicy);
        
        return services;
    }
}

public interface IResillientHttpClient
{
    Task<HttpResponseMessage> GetAsync(string url);
    Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
}

public class ResiliantHttpClient : IResillientHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IAsyncPolicy<HttpResponseMessage> _policy;
    
    public ResiliantHttpClient(
        HttpClient httpClient,
        IAsyncPolicy<HttpResponseMessage> retryPolicy,
        IAsyncPolicy<HttpResponseMessage> circuitBreakerPolicy)
    {
        _httpClient = httpClient;
        
        _policy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
    }
    
    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        return await _policy.ExecuteAsync(async () => await _httpClient.GetAsync(url));
    }
    
    public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
    {
        return await _policy.ExecuteAsync(async () => await _httpClient.PostAsync(url, content));
    }
}
