using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelRequestService.Application.Interfaces;
using TravelRequestService.Domain.Interfaces;
using TravelRequestService.Infrastructure.BlobStorage;
using TravelRequestService.Infrastructure.Dapper;
using TravelRequestService.Infrastructure.Data;
using TravelRequestService.Infrastructure.Messaging;
using TravelRequestService.Infrastructure.Messaging.Consumers;
using TravelRequestService.Infrastructure.Repositories;

namespace TravelRequestService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<TravelDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("TravelDb"),
                b => b.MigrationsAssembly(typeof(TravelDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ITravelRequestRepository, TravelRequestRepository>();
        services.AddScoped<ITravelAdvanceRepository, TravelAdvanceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Messaging
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // Message Consumers
        services.AddHostedService<TravelRequestApprovalConsumer>();
        services.AddHostedService<TravelAdvancePaymentConsumer>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        return services;
    }
}
