using EmployeePrideManagement.Domain.Interfaces;
using EmployeePrideManagement.Infrastructure.Data.Context;
using EmployeePrideManagement.Infrastructure.HealthChecks;
using EmployeePrideManagement.Infrastructure.Messaging;
using EmployeePrideManagement.Infrastructure.Repositories;
using EmployeePrideManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeePrideManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<PrideManagementDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(PrideManagementDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IPrideMomentRepository, PrideMomentRepository>();
        services.AddScoped<IDapperPrideMomentRepository, DapperPrideMomentRepository>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ
        services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();

        // RabbitMQ Consumers
        services.AddHostedService<PrideMomentCreatedConsumer>();
        services.AddHostedService<PrideMomentUpdatedConsumer>();

        // Health Checks
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("database", tags: new[] { "db", "sql" })
            .AddCheck<RabbitMqHealthCheck>("rabbitmq", tags: new[] { "messaging" });

        return services;
    }
}
