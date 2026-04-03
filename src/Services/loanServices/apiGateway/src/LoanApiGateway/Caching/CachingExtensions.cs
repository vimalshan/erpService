namespace LoanApiGateway.Caching;

/// <summary>
/// Configures response caching and memory cache for cacheable GET routes.
/// The YARP pipeline respects standard Cache-Control headers from downstream services.
/// </summary>
public static class CachingExtensions
{
    public static IServiceCollection AddGatewayCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMemoryCache(options =>
        {
            options.SizeLimit = configuration.GetValue("Gateway:Cache:SizeLimitMb", 128) * 1024L * 1024L;
        });

        services.AddResponseCaching(options =>
        {
            options.MaximumBodySize = configuration.GetValue("Gateway:Cache:MaxBodySizeBytes", 1_048_576); // 1 MB
            options.UseCaseSensitivePaths = false;
        });

        services.AddOutputCache(options =>
        {
            // Default: cache all GET/HEAD responses for 60 s (respects Vary, Cache-Control)
            options.AddBasePolicy(builder =>
                builder.With(c => c.HttpContext.Request.Method == HttpMethods.Get
                                  || c.HttpContext.Request.Method == HttpMethods.Head)
                       .Expire(TimeSpan.FromSeconds(
                           configuration.GetValue("Gateway:Cache:DefaultExpirySeconds", 60))));

            // Short-lived cache for list endpoints (loan definitions, LOVs)
            options.AddPolicy("list-cache", builder =>
                builder.Expire(TimeSpan.FromSeconds(
                    configuration.GetValue("Gateway:Cache:ListExpirySeconds", 30))));

            // Longer cache for reference data (LOV values, loan type definitions)
            options.AddPolicy("reference-cache", builder =>
                builder.Expire(TimeSpan.FromSeconds(
                    configuration.GetValue("Gateway:Cache:ReferenceExpirySeconds", 300))));

            // No cache for auth and mutation endpoints
            options.AddPolicy("no-cache", builder => builder.NoCache());
        });

        return services;
    }
}
