using HealthTransaction.Domain.Interfaces;
using HealthTransaction.Infrastructure.Dapper;
using HealthTransaction.Infrastructure.Messaging;
using HealthTransaction.Infrastructure.Messaging.Consumers;
using HealthTransaction.Infrastructure.Persistence;
using HealthTransaction.Infrastructure.Repositories;
using HealthTransaction.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthTransaction.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is not configured.");

        services.AddDbContext<HealthTransactionDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPreEmploymentCheckupRepository, PreEmploymentCheckupRepository>();
        services.AddScoped<ICheckupCardRepository, CheckupCardRepository>();
        services.AddScoped<IDynamicHealthDetailRepository, DynamicHealthDetailRepository>();
        services.AddScoped<IPfiHistoryRepository, PfiHistoryRepository>();
        services.AddSingleton(_ => new DapperQueryService(connectionString));
        services.AddSingleton<BlobStorageService>();

        // RabbitMQ publisher
        var rabbitHostName = configuration["RabbitMQ:HostName"] ?? "localhost";
        var rabbitPort = int.TryParse(configuration["RabbitMQ:Port"], out var p) ? p : 5672;
        var rabbitUser = configuration["RabbitMQ:Username"] ?? "guest";
        var rabbitPass = configuration["RabbitMQ:Password"] ?? "guest";
        services.AddSingleton<IMessagePublisher>(_ =>
            new RabbitMqPublisher(rabbitHostName, rabbitPort, rabbitUser, rabbitPass));

        // RabbitMQ consumer
        services.AddHostedService<HealthTransactionEventConsumer>();

        return services;
    }
}
