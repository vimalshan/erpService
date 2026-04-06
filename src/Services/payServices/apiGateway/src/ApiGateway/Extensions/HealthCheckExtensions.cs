namespace ApiGateway.Extensions;

/// <summary>
/// Configures health checks for the gateway and all downstream services.
/// </summary>
public static class HealthCheckExtensions
{
    public static IServiceCollection AddGatewayHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddUrlGroup(new Uri("http://localhost:5104/health"), name: "employee-service",
                timeout: TimeSpan.FromSeconds(5),
                tags: ["services", "employee"])
            .AddUrlGroup(new Uri("http://localhost:5000/health"), name: "hr-service",
                timeout: TimeSpan.FromSeconds(5),
                tags: ["services", "hr"])
            .AddUrlGroup(new Uri("http://localhost:5032/health"), name: "faq-service",
                timeout: TimeSpan.FromSeconds(5),
                tags: ["services", "faq"])
            .AddUrlGroup(new Uri("http://localhost:5002/health"), name: "payroll-service",
                timeout: TimeSpan.FromSeconds(5),
                tags: ["services", "payroll"])
            .AddUrlGroup(new Uri("http://localhost:5010/health"), name: "tax-service",
                timeout: TimeSpan.FromSeconds(5),
                tags: ["services", "tax"])
            .AddUrlGroup(new Uri("http://localhost:5020/health"), name: "paytransactional-service",
                timeout: TimeSpan.FromSeconds(5),
                tags: ["services", "paytransactional"]);

        return services;
    }
}
