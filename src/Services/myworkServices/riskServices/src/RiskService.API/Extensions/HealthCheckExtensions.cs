using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace RiskService.API.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddRiskHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        var builder = services.AddHealthChecks()
            .AddSqlServer(connectionString, name: "database", tags: new[] { "db", "sql" });

        var rabbitHost = configuration.GetValue<string>("RabbitMQ:HostName");
        if (!string.IsNullOrEmpty(rabbitHost))
        {
            var rabbitUser = configuration.GetValue<string>("RabbitMQ:UserName") ?? "guest";
            var rabbitPass = configuration.GetValue<string>("RabbitMQ:Password") ?? "guest";
            builder.AddRabbitMQ(sp =>
            {
                var factory = new RabbitMQ.Client.ConnectionFactory
                {
                    HostName = rabbitHost,
                    UserName = rabbitUser,
                    Password = rabbitPass
                };
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            },
            name: "rabbitmq",
            tags: new[] { "messaging" });
        }

        return services;
    }
}
