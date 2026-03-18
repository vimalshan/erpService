using Azure.Storage.Blobs;
using LovService.Application.Interfaces;
using LovService.Infrastructure.Adapters;
using LovService.Infrastructure.Data;
using LovService.Infrastructure.DapperRepositories;
using LovService.Infrastructure.Messaging;
using LovService.Infrastructure.Repositories;
using LovService.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace LovService.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Entity Framework
        var connectionString = configuration.GetConnectionString("LovDb")
            ?? throw new InvalidOperationException("Connection string 'LovDb' not found.");

        services.AddDbContext<LovDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
            {
                sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                sql.CommandTimeout(30);
                sql.MigrationsAssembly(typeof(LovDbContext).Assembly.FullName);
            }));

        // Repositories and Unit of Work
        services.AddScoped<ILovTypeRepository, LovTypeRepository>();
        services.AddScoped<ILovMasterRepository, LovMasterRepository>();
        services.AddScoped<IItemDataRepository, ItemDataRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // Dapper
        services.AddSingleton<LovTypeDapperRepository>();

        // RabbitMQ — optional: skipped if broker is unreachable
        var rabbitHost = configuration["RabbitMQ:Host"] ?? "localhost";
        var rabbitPort = configuration.GetValue<int>("RabbitMQ:Port", 5672);
        var rabbitUser = configuration["RabbitMQ:Username"] ?? "guest";
        var rabbitPass = configuration["RabbitMQ:Password"] ?? "guest";
        var rabbitEnabled = configuration.GetValue<bool>("RabbitMQ:Enabled", true);
        if (rabbitEnabled)
        {
            IConnection? rabbitConnection = null;
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = rabbitHost,
                    Port     = rabbitPort,
                    UserName = rabbitUser,
                    Password = rabbitPass
                };
                rabbitConnection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WRN] RabbitMQ is unavailable at {rabbitHost}:{rabbitPort}. Messaging features will be disabled. {ex.Message}");
            }

            if (rabbitConnection is not null)
            {
                services.AddSingleton<IConnection>(rabbitConnection);
                services.AddSingleton<RabbitMQPublisher>();
                services.AddHostedService<RabbitMQConsumer>();
            }
        }

        // Azure Blob Storage
        var blobConnectionString = configuration.GetConnectionString("BlobStorage");
        if (!string.IsNullOrWhiteSpace(blobConnectionString))
        {
            services.AddSingleton(_ => new BlobServiceClient(blobConnectionString));
            services.AddSingleton<BlobStorageAdapter>();
        }

        return services;
    }
}
