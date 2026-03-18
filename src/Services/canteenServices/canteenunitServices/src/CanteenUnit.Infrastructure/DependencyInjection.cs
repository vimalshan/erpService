using CanteenUnit.Application.Common.Interfaces;
using CanteenUnit.Domain.Interfaces;
using CanteenUnit.Infrastructure.Dapper;
using CanteenUnit.Infrastructure.Messaging;
using CanteenUnit.Infrastructure.Messaging.Consumers;
using CanteenUnit.Infrastructure.Persistence;
using CanteenUnit.Infrastructure.Repositories;
using CanteenUnit.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CanteenUnit.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(p => p.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<ICanteenUnitRepository, CanteenUnitRepository>();
        services.AddScoped<ICanteenMasterRepository, CanteenMasterRepository>();
        services.AddScoped<IGenCounterRepository, GenCounterRepository>();

        // Dapper
        services.AddSingleton<IDapperContext, DapperContext>();
        services.AddScoped<CanteenUnitDapperRepository>();

        // RabbitMQ
        services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();
        services.AddHostedService<CanteenUnitConsumer>();

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        return services;
    }
}
