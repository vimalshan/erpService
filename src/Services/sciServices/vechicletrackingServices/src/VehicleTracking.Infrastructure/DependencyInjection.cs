using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VehicleTracking.Domain.Interfaces;
using VehicleTracking.Infrastructure.Dapper;
using VehicleTracking.Infrastructure.Messaging;
using VehicleTracking.Infrastructure.Persistence;
using VehicleTracking.Infrastructure.Storage;

namespace VehicleTracking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<VehicleTrackingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(3)));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ Publisher
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // RabbitMQ Consumers
        services.AddHostedService<VehicleStageConsumer>();
        services.AddHostedService<DecisionConsumer>();

        return services;
    }
}
