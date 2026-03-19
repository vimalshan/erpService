using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductionManagement.Application.Interfaces;
using ProductionManagement.Domain.Interfaces;
using ProductionManagement.Infrastructure.Dapper;
using ProductionManagement.Infrastructure.Messaging;
using ProductionManagement.Infrastructure.Messaging.Consumers;
using ProductionManagement.Infrastructure.Persistence;
using ProductionManagement.Infrastructure.Repositories;
using ProductionManagement.Infrastructure.Storage;

namespace ProductionManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ProductionManagementDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ProductionManagementDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductionPlantRepository, ProductionPlantRepository>();
        services.AddScoped<IProductionPlanRepository, ProductionPlanRepository>();
        services.AddScoped<INormsRepository, NormsRepository>();
        services.AddScoped<IMamProductionRepository, MamProductionRepository>();

        // Dapper
        services.AddScoped<IDapperContext, DapperContext>();
        services.AddScoped<IProductionDapperQueries, ProductionDapperQueries>();

        // RabbitMQ
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // RabbitMQ Consumers
        services.AddHostedService<ProductionPlanUpdatedConsumer>();
        services.AddHostedService<ProductionPlantCreatedConsumer>();

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        return services;
    }
}
