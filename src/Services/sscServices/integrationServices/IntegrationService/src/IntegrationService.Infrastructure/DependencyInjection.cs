using IntegrationService.Application.Interfaces;
using IntegrationService.Domain.Interfaces;
using IntegrationService.Infrastructure.Dapper;
using IntegrationService.Infrastructure.Messaging;
using IntegrationService.Infrastructure.Messaging.Consumers;
using IntegrationService.Infrastructure.Persistence;
using IntegrationService.Infrastructure.Persistence.Repositories;
using IntegrationService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IntegrationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<IntegrationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IntegrationDb")));

        // Unit of Work
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<IntegrationDbContext>());

        // Repositories
        services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IVendorSiteRepository, VendorSiteRepository>();
        services.AddScoped<IMaterialReceiptRepository, MaterialReceiptRepository>();
        services.AddScoped<IOrganizationUnitRepository, OrganizationUnitRepository>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ Publisher
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            try
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var logger = sp.GetRequiredService<ILogger<RabbitMqPublisher>>();
                return RabbitMqPublisher.CreateAsync(config, logger).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                var logger = sp.GetRequiredService<ILogger<NullMessagePublisher>>();
                logger.LogWarning(ex, "RabbitMQ unavailable. Using NullMessagePublisher.");
                return new NullMessagePublisher(logger);
            }
        });

        // RabbitMQ Consumers
        services.AddHostedService<PurchaseOrderSyncConsumer>();
        services.AddHostedService<VendorSyncConsumer>();

        return services;
    }
}
