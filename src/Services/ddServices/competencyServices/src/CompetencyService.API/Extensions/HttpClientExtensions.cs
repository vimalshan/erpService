using Polly;
using Polly.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using CompetencyService.Infrastructure.Resilience;

namespace CompetencyService.API.Extensions;

public static class HttpClientExtensions
{
    /// <summary>
    /// Registers a typed HttpClient with retry (3x exponential backoff)
    /// and circuit-breaker (5 failures → 30s break) policies.
    /// </summary>
    public static IHttpClientBuilder AddResilientHttpClient<TClient, TImplementation>(
        this IServiceCollection services, string baseAddress)
        where TClient : class
        where TImplementation : class, TClient
    {
        return services
            .AddHttpClient<TClient, TImplementation>(c => c.BaseAddress = new Uri(baseAddress))
            .AddPolicyHandler(PollyPolicies.GetRetryPolicy())
            .AddPolicyHandler(PollyPolicies.GetCircuitBreakerPolicy());
    }
}
