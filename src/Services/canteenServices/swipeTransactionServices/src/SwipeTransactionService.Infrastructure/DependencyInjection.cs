using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SwipeTransactionService.Domain.Interfaces.Repositories;
using SwipeTransactionService.Infrastructure.Auth;
using SwipeTransactionService.Infrastructure.Dapper;
using SwipeTransactionService.Infrastructure.Messaging;
using SwipeTransactionService.Infrastructure.Persistence;
using SwipeTransactionService.Infrastructure.Persistence.Repositories;
using SwipeTransactionService.Infrastructure.Services;
using SwipeTransactionService.Infrastructure.Storage;

namespace SwipeTransactionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<SwipeTransactionDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(SwipeTransactionDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ISwipeCardUploadRepository, SwipeCardUploadRepository>();
        services.AddScoped<ICanteenPunchRepository, CanteenPunchRepository>();
        services.AddScoped<IDailyAvailedRepository, DailyAvailedRepository>();
        services.AddScoped<ICanteenDaconRepository, CanteenDaconRepository>();

        // Dapper read service
        services.AddSingleton(sp =>
            new SwipeReportQueryService(configuration.GetConnectionString("DefaultConnection")!));

        // Domain event dispatcher
        services.AddScoped<DomainEventDispatcher>();

        // JWT
        var jwtSection = configuration.GetSection("Jwt");
        services.AddSingleton(new JwtTokenService(
            jwtSection["Secret"]!,
            jwtSection["Issuer"]!,
            jwtSection["Audience"]!,
            int.Parse(jwtSection["ExpiryMinutes"] ?? "60")));

        // RabbitMQ
        var rabbitCfg = configuration.GetSection("RabbitMQ");
        services.AddSingleton<IConnectionFactory>(_ => new ConnectionFactory
        {
            HostName = rabbitCfg["Host"] ?? "localhost",
            Port = int.Parse(rabbitCfg["Port"] ?? "5672"),
            UserName = rabbitCfg["Username"] ?? "guest",
            Password = rabbitCfg["Password"] ?? "guest",
            VirtualHost = rabbitCfg["VirtualHost"] ?? "/"
        });

        services.AddSingleton<IConnection>(sp =>
        {
            var factory = sp.GetRequiredService<IConnectionFactory>();
            try
            {
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                var logger = sp.GetService<Microsoft.Extensions.Logging.ILogger<RabbitMqPublisher>>();
                logger?.LogWarning(ex, "RabbitMQ connection unavailable at startup. Messaging will be disabled.");
                return null!;
            }
        });

        services.AddSingleton<IChannel>(sp =>
        {
            var conn = sp.GetService<IConnection>();
            if (conn is null) return null!;
            try
            {
                return conn.CreateChannelAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                var logger = sp.GetService<Microsoft.Extensions.Logging.ILogger<RabbitMqPublisher>>();
                logger?.LogWarning(ex, "RabbitMQ channel creation failed.");
                return null!;
            }
        });

        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddSingleton<SwipeTransactionConsumer>();

        // Azure Blob Storage
        var blobConnStr = configuration.GetConnectionString("BlobStorage");
        if (!string.IsNullOrWhiteSpace(blobConnStr))
        {
            services.AddSingleton(new BlobServiceClient(blobConnStr));
            services.AddSingleton<IBlobStorageService, BlobStorageService>();
        }

        return services;
    }
}
