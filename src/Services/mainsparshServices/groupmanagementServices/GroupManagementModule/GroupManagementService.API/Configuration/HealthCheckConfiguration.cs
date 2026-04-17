namespace GroupManagementService.API.Configuration
{
    public static class HealthCheckConfiguration
    {
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("DefaultConnection");
            
            services
                .AddHealthChecks()
                .AddSqlServer(connString ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured in appsettings.json"), 
                    name: "Database", 
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy);

            return services;
        }

        public static WebApplication MapHealthCheckEndpoints(this WebApplication app)
        {
            app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(e => new
                        {
                            name = e.Key,
                            status = e.Value.Status.ToString(),
                            description = e.Value.Description
                        })
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            });

            app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = _ => false
            });

            return app;
        }
    }
}
