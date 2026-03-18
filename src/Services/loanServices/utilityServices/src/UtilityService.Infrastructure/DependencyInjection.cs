using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using UtilityService.Domain.Interfaces;
using UtilityService.Infrastructure.Data;
using UtilityService.Infrastructure.HealthChecks;
using UtilityService.Infrastructure.Messaging;
using UtilityService.Infrastructure.Messaging.Consumers;
using UtilityService.Infrastructure.Repositories;
using UtilityService.Infrastructure.Storage;

namespace UtilityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IToadPlanSqlRepository, ToadPlanSqlRepository>();
        services.AddScoped<ToadPlanSqlDapperRepository>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // RabbitMQ
        services.Configure<RabbitMQSettings>(options =>
        {
            options.Host = configuration["RabbitMQ:Host"] ?? "localhost";
            options.Port = int.TryParse(configuration["RabbitMQ:Port"], out var port) ? port : 5672;
            options.VirtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/";
            options.Username = configuration["RabbitMQ:Username"] ?? "guest";
            options.Password = configuration["RabbitMQ:Password"] ?? "guest";
            options.ExchangeName = configuration["RabbitMQ:ExchangeName"] ?? "utility.exchange";
            options.QueueName = configuration["RabbitMQ:QueueName"] ?? "utility.toadplan.events";
            options.DeadLetterExchange = configuration["RabbitMQ:DeadLetterExchange"] ?? "utility.dlx";
        });
        services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();
        services.AddHostedService<ToadPlanSqlCreatedConsumer>();

        // RabbitMQ connection factory for health checks
        services.AddSingleton<IConnectionFactory>(_ =>
        {
            var host = configuration["RabbitMQ:Host"] ?? "localhost";
            var port = int.TryParse(configuration["RabbitMQ:Port"], out var p) ? p : 5672;
            var user = configuration["RabbitMQ:Username"] ?? "guest";
            var pass = configuration["RabbitMQ:Password"] ?? "guest";
            return new ConnectionFactory
            {
                HostName = host,
                Port = port,
                UserName = user,
                Password = pass
            };
        });

        // Health Checks
        var connStr = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection string missing.");

        services.AddHealthChecks()
            .AddSqlServer(connectionString: connStr, name: "sqlserver", tags: ["db", "sql"])
            .AddCheck<RabbitMQHealthCheck>("rabbitmq", tags: ["messaging"]);

        return services;
    }
}

