using InvestmentService.Domain.Interfaces;
using InvestmentService.Infrastructure.Data;
using InvestmentService.Infrastructure.Repositories;
using InvestmentService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvestmentService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<InvestmentDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("InvestmentDb"),
                b => b.MigrationsAssembly(typeof(InvestmentDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // RabbitMQ Publisher
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ Consumers
        services.AddHostedService<InvestmentMaturityConsumer>();
        services.AddHostedService<InvestmentRedemptionConsumer>();
        services.AddHostedService<InvestmentApprovalConsumer>();

        return services;
    }
}
