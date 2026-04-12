using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using travelTransactionService.Application.Interfaces;
using travelTransactionService.Domain.Interfaces;
using travelTransactionService.Infrastructure.BlobStorage;
using travelTransactionService.Infrastructure.Dapper;
using travelTransactionService.Infrastructure.Data;
using travelTransactionService.Infrastructure.Messaging;
using travelTransactionService.Infrastructure.Messaging.Consumers;
using travelTransactionService.Infrastructure.Repositories;

namespace travelTransactionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<TransactionDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("TravelDb"),
                b => b.MigrationsAssembly(typeof(TransactionDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IVendorMasterRepository, VendorMasterRepository>();
        services.AddScoped<ITaxMasterRepository, TaxMasterRepository>();
        services.AddScoped<IJaiInterfaceLineRepository, JaiInterfaceLineRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Messaging
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // Message Consumers
        services.AddHostedService<VendorUpdateConsumer>();
        services.AddHostedService<TaxCalculationConsumer>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // MediatR for domain event handlers in this assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
