namespace ApiGateway.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddGatewayHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddUrlGroup(new Uri("http://localhost:5205/health"), name: "travel-request-service",
                tags: new[] { "service", "travel" })
            .AddUrlGroup(new Uri("http://localhost:5082/health"), name: "travel-transaction-service",
                tags: new[] { "service", "transaction" })
            .AddUrlGroup(new Uri("http://localhost:5117/health"), name: "booking-service",
                tags: new[] { "service", "booking" })
            .AddUrlGroup(new Uri("http://localhost:5090/health"), name: "expense-service",
                tags: new[] { "service", "expense" })
            .AddUrlGroup(new Uri("http://localhost:5294/health"), name: "finance-service",
                tags: new[] { "service", "finance" })
            .AddUrlGroup(new Uri("http://localhost:5179/health"), name: "insurance-service",
                tags: new[] { "service", "insurance" })
            .AddUrlGroup(new Uri("http://localhost:5166/health"), name: "masterdata-service",
                tags: new[] { "service", "masterdata" })
            .AddUrlGroup(new Uri("http://localhost:5000/health"), name: "agency-service",
                tags: new[] { "service", "agency" })
            .AddUrlGroup(new Uri("http://localhost:5001/health"), name: "admin-service",
                tags: new[] { "service", "admin" });

        return services;
    }
}
