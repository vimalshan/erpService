namespace ApiGateway.Extensions;

public static class CachingExtensions
{
    public static IServiceCollection AddGatewayResponseCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var durationSeconds = configuration.GetValue<int>("ResponseCache:DefaultDurationSeconds", 60);

        services.AddResponseCaching(options =>
        {
            options.MaximumBodySize  = 64 * 1024; // 64 KB per response
            options.SizeLimit        = 100 * 1024 * 1024; // 100 MB total cache
            options.UseCaseSensitivePaths = false;
        });

        services.AddMemoryCache(options =>
        {
            options.SizeLimit = 1024;
        });

        services.AddOutputCache(options =>
        {
            options.AddBasePolicy(builder =>
                builder.Cache()
                       .Expire(TimeSpan.FromSeconds(durationSeconds))
                       .SetVaryByQuery("*"));

            options.AddPolicy("no-cache", builder => builder.NoCache());

            options.AddPolicy("short", builder =>
                builder.Cache()
                       .Expire(TimeSpan.FromSeconds(15))
                       .SetVaryByQuery("*"));

            options.AddPolicy("long", builder =>
                builder.Cache()
                       .Expire(TimeSpan.FromSeconds(300))
                       .SetVaryByQuery("*"));
        });

        return services;
    }
}
