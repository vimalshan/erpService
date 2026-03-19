using MamAllocationService.Application.Handlers;
using MamAllocationService.Application.Interfaces;
using MamAllocationService.Domain.Interfaces;
using MamAllocationService.Infrastructure.BlobStorage;
using MamAllocationService.Infrastructure.Dapper;
using MamAllocationService.Infrastructure.Messaging;
using MamAllocationService.Infrastructure.Persistence;
using MamAllocationService.Infrastructure.Repositories;
using MamAllocationService.Infrastructure.Resilience;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MamAllocationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MamAllocationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAllocationDetailRepository, AllocationDetailRepository>();
        services.AddScoped<IAllocationProdDetailRepository, AllocationProdDetailRepository>();
        services.AddScoped<IAllocationFgRepository, AllocationFgRepository>();
        services.AddScoped<IArrivalDetailRepository, ArrivalDetailRepository>();
        services.AddScoped<IConsumptionDetailRepository, ConsumptionDetailRepository>();
        services.AddScoped<IDispatchDetailRepository, DispatchDetailRepository>();
        services.AddScoped<IFgAllocationRepository, FgAllocationRepository>();
        services.AddScoped<IProductAllocationRepository, ProductAllocationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IAllocationSummaryDapperQuery, AllocationSummaryDapperQuery>();

        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

        services.AddHostedService<AllocationCreatedConsumer>();
        services.AddHostedService<ArrivalCreatedConsumer>();
        services.AddHostedService<DispatchCreatedConsumer>();

        services.AddResiliencePolicies();

        return services;
    }
}
