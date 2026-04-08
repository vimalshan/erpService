using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PFTransactionalService.Domain.Interfaces;
using PFTransactionalService.Infrastructure.BlobStorage;
using PFTransactionalService.Infrastructure.Messaging;
using PFTransactionalService.Infrastructure.Persistence.Dapper;
using PFTransactionalService.Infrastructure.Persistence.EfCore;
using PFTransactionalService.Infrastructure.Repositories;

namespace PFTransactionalService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<PFTransactionalDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("PFTransactionalDb"),
                b => b.MigrationsAssembly(typeof(PFTransactionalDbContext).Assembly.FullName)));

        // Dapper queries
        services.AddScoped<PFTransactionalDapperQueries>();

        // Repositories
        services.AddScoped<IPFAccumulationRepository, PFAccumulationRepository>();
        services.AddScoped<IPFSettlementRepository, PFSettlementRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Messaging
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<PFTransactionMessageConsumer>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        return services;
    }
}
