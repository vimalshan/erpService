using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CompensationService.API.Configuration;

/// <summary>
/// Extension method to configure resilience policies for HTTP clients
/// </summary>
public static class ResiliencePoliciesExtensions
{
    public static IHttpClientBuilder AddResiliencePolicies(this IHttpClientBuilder builder)
    {
        // Configure basic timeouts for HTTP clients
        builder.ConfigureHttpClient(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return builder;
    }
}
