namespace WebsiteContentService.API.Configuration;

public static class ResiliencePoliciesExtensions
{
    public static IHttpClientBuilder AddResiliencePolicies(this IHttpClientBuilder builder)
    {
        builder.ConfigureHttpClient(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return builder;
    }
}
