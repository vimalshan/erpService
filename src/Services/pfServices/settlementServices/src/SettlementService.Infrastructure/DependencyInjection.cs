using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SettlementService.Domain.Interfaces;
using SettlementService.Infrastructure.BlobStorage;
using SettlementService.Infrastructure.Messaging;
using SettlementService.Infrastructure.Persistence.Dapper;
using SettlementService.Infrastructure.Persistence.EfCore;
using SettlementService.Infrastructure.Repositories;

namespace SettlementService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<SettlementDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("SettlementDb"),
                b => b.MigrationsAssembly(typeof(SettlementDbContext).Assembly.FullName)));

        // Dapper queries
        services.AddScoped<SettlementDapperQueries>();

        // Repositories
        services.AddScoped<ISettlementRepository, SettlementRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Messaging
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<SettlementMessageConsumer>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        return services;
    }
}
