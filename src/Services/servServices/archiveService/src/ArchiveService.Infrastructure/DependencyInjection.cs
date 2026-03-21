using ArchiveService.Application.Interfaces;
using ArchiveService.Domain.Interfaces;
using ArchiveService.Infrastructure.Dapper;
using ArchiveService.Infrastructure.Messaging;
using ArchiveService.Infrastructure.Persistence;
using ArchiveService.Infrastructure.Repositories;
using ArchiveService.Infrastructure.Resilience;
using ArchiveService.Infrastructure.Services;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using RabbitMQ.Client;

namespace ArchiveService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ArchiveDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("ArchiveConnection"),
                b => b.MigrationsAssembly(typeof(ArchiveDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IArchivedServiceOrderRepository, ArchivedServiceOrderRepository>();
        services.AddScoped<IArchivedServiceOrderDetailRepository, ArchivedServiceOrderDetailRepository>();
        services.AddScoped<IArchivedToolKitRepository, ArchivedToolKitRepository>();
        services.AddScoped<IArchivedToolKitTransactionRepository, ArchivedToolKitTransactionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddSingleton<DapperQueryService>();

        // Blob Storage
        var blobConnectionString = configuration.GetConnectionString("BlobStorage");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        // RabbitMQ
        var rabbitConfig = configuration.GetSection("RabbitMQ");
        services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory
            {
                HostName = rabbitConfig["HostName"] ?? "localhost",
                UserName = rabbitConfig["UserName"] ?? "guest",
                Password = rabbitConfig["Password"] ?? "guest",
                Port = int.TryParse(rabbitConfig["Port"], out var port) ? port : 5672
            };
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        services.AddScoped<IMessagePublisher, RabbitMqPublisher>();

        // Message consumers
        services.AddHostedService<ArchiveOrderConsumer>();
        services.AddHostedService<ArchivePurgeConsumer>();

        // Polly resilience pipelines
        services.AddSingleton(PollyPolicies.GetDatabaseResiliencePipeline());

        return services;
    }
}
