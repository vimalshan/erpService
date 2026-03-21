namespace TransactionService.Infrastructure.Resilience;

using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.CircuitBreaker;
using Polly.Retry;

public static class ResiliencePolicies
{
    private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy =
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy =
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30));

    public static IServiceCollection AddResiliencePolicies(this IServiceCollection services)
    {
        services.AddSingleton(RetryPolicy);
        services.AddSingleton(CircuitBreakerPolicy);

        // Named HttpClient with policies
        services.AddHttpClient("TransactionServiceClient")
            .AddPolicyHandler(RetryPolicy)
            .AddPolicyHandler(CircuitBreakerPolicy);

        return services;
    }

    public static IHttpClientBuilder AddResilienceHandler(this IHttpClientBuilder builder)
    {
        return builder
            .AddPolicyHandler(RetryPolicy)
            .AddPolicyHandler(CircuitBreakerPolicy);
    }
}
