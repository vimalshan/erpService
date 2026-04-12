namespace ApiGateway.Extensions;

public static class CachingExtensions
{
    public static IServiceCollection AddGatewayResponseCaching(this IServiceCollection services)
    {
        services.AddResponseCaching(options =>
        {
            options.MaximumBodySize = 64 * 1024 * 1024; // 64 MB
            options.UseCaseSensitivePaths = false;
        });

        services.AddOutputCache(options =>
        {
            // Default policy: cache GET requests for 30 seconds
            options.AddBasePolicy(builder => builder
                .With(c => c.HttpContext.Request.Method == "GET")
                .Expire(TimeSpan.FromSeconds(30))
                .Tag("default"));

            // Master data: cache longer (5 minutes)
            options.AddPolicy("masterdata", builder => builder
                .Expire(TimeSpan.FromMinutes(5))
                .Tag("masterdata"));

            // Lookup data: cache 10 minutes
            options.AddPolicy("lookups", builder => builder
                .Expire(TimeSpan.FromMinutes(10))
                .Tag("lookups"));

            // No cache for mutations
            options.AddPolicy("nocache", builder => builder.NoCache());
        });

        return services;
    }
}
