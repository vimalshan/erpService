using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PurchaseSalesService.Application.Common.Interfaces;
using PurchaseSalesService.Domain.Interfaces;
using PurchaseSalesService.Infrastructure.DapperRepositories;
using PurchaseSalesService.Infrastructure.Data;
using PurchaseSalesService.Infrastructure.Messaging;
using PurchaseSalesService.Infrastructure.Messaging.Consumers;
using PurchaseSalesService.Infrastructure.Repositories;
using PurchaseSalesService.Infrastructure.Storage;

namespace PurchaseSalesService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        // EF Repositories
        services.AddScoped<IPurchaseRepository, PurchaseRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();

        // Dapper read repositories
        services.AddScoped<PurchaseDapperRepository>();
        services.AddScoped<SaleDapperRepository>();

        // RabbitMQ Publisher
        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();

        // RabbitMQ Consumers
        services.AddHostedService<PurchaseCreatedConsumer>();
        services.AddHostedService<SaleCreatedConsumer>();

        // Azure Blob Storage
        services.AddSingleton<BlobStorageService>();

        return services;
    }
}
