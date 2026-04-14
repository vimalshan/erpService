using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseStructure.Application.Interfaces;
using WarehouseStructure.Domain.Interfaces;
using WarehouseStructure.Infrastructure.Dapper;
using WarehouseStructure.Infrastructure.Persistence;
using WarehouseStructure.Infrastructure.Repositories;
using WarehouseStructure.Infrastructure.Services;

namespace WarehouseStructure.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<WarehouseStructureDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(WarehouseStructureDbContext).Assembly.FullName)));

        // Dapper
        services.AddSingleton<DapperContext>();
        services.AddScoped<WarehouseDapperRepository>();

        // Repositories
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IZoneRepository, ZoneRepository>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // MediatR — register domain event handlers from Infrastructure assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // RabbitMQ
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
        services.AddHostedService<WarehouseMessageConsumer>();

        return services;
    }
}
