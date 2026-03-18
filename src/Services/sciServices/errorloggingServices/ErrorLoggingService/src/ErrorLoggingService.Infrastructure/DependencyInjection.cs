using ErrorLoggingService.Domain.Repositories;
using ErrorLoggingService.Infrastructure.DapperRepositories;
using ErrorLoggingService.Infrastructure.Messaging.Consumers;
using ErrorLoggingService.Infrastructure.Messaging.Publishers;
using ErrorLoggingService.Infrastructure.Persistence;
using ErrorLoggingService.Infrastructure.Repositories;
using ErrorLoggingService.Infrastructure.Storage;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ErrorLoggingService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
        services.AddScoped<ErrorLogDapperRepository>();

        // Blob Storage
        services.AddSingleton<BlobStorageService>();

        // MassTransit: use RabbitMQ when enabled, otherwise InMemory (safe for dev/CI)
        var rabbitEnabled = bool.TryParse(configuration["RabbitMQ:Enabled"], out var re) && re;

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ErrorLogNotificationConsumer>();

            if (rabbitEnabled)
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(
                        configuration["RabbitMQ:Host"] ?? "localhost",
                        "/",
                        h =>
                        {
                            h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                            h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                        });

                    cfg.ReceiveEndpoint("error-log-notifications", e =>
                    {
                        e.ConfigureConsumer<ErrorLogNotificationConsumer>(ctx);
                    });
                });
            }
            else
            {
                x.UsingInMemory((ctx, cfg) =>
                {
                    cfg.ConfigureEndpoints(ctx);
                });
            }
        });

        services.AddScoped<ErrorLogMessagePublisher>();

        return services;
    }
}
