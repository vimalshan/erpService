using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CanteenTransactionService.Domain.Interfaces;
using CanteenTransactionService.Infrastructure.DomainEvents;
using CanteenTransactionService.Infrastructure.Messaging.Consumers;
using CanteenTransactionService.Infrastructure.Messaging.RabbitMQ;
using CanteenTransactionService.Infrastructure.Persistence.Dapper;
using CanteenTransactionService.Infrastructure.Persistence.EF;
using CanteenTransactionService.Infrastructure.Persistence.Repositories;
using CanteenTransactionService.Infrastructure.Resilience;
using CanteenTransactionService.Infrastructure.Storage;

namespace CanteenTransactionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        // EF Core
        services.AddDbContext<CanteenTransactionDbContext>(options =>
            options.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(CanteenTransactionDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ICanteenDaconRepository, CanteenDaconRepository>();
        services.AddScoped<IDailyAvailedRepository, DailyAvailedRepository>();
        services.AddScoped<IMisBatchSubmissionRepository, MisBatchSubmissionRepository>();

        // Dapper
        services.AddSingleton(sp => new TransactionDapperRepository(
            config.GetConnectionString("DefaultConnection")!));

        // Blob Storage
        services.Configure<BlobStorageSettings>(config.GetSection("BlobStorage"));
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // RabbitMQ
        services.Configure<RabbitMQSettings>(config.GetSection("RabbitMQ"));
        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();

        // Message Consumers (Background Services)
        services.AddHostedService<CanteenTransactionConsumer>();

        // Resilience
        services.AddSingleton<ResiliencePolicies>();

        // Domain Event Dispatcher
        services.AddScoped<DomainEventDispatcher>();

        return services;
    }
}
