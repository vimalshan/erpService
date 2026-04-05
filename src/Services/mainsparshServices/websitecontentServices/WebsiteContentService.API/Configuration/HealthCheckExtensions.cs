namespace WebsiteContentService.API.Configuration;

using Microsoft.Extensions.Diagnostics.HealthChecks;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthChecksConfiguration(this IServiceCollection services, string connectionString)
    {
        services.AddHealthChecks()
            .AddSqlServer(
                connectionString,
                name: "Database",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["database", "sql"])
            .AddCheck("API Health", () => HealthCheckResult.Healthy("API is running"), tags: ["api"]);

        return services;
    }

    public static IApplicationBuilder UseHealthChecksConfiguration(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var response = new
                {
                    status = report.Status.ToString(),
                    details = report.Entries.ToDictionary(x => x.Key, x => new { status = x.Value.Status.ToString() })
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        });

        app.UseHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("database"),
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var response = new { status = report.Status.ToString() };
                await context.Response.WriteAsJsonAsync(response);
            }
        });

        return app;
    }
}
