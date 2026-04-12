using Azure.Storage.Blobs;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Infrastructure.Messaging;
using FinanceService.Infrastructure.Messaging.Consumers;
using FinanceService.Infrastructure.Persistence;
using FinanceService.Infrastructure.Repositories;
using FinanceService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace FinanceService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("FinanceDb")!;

        // EF Core
        services.AddDbContext<FinanceDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IFinanceDbContext>(provider => provider.GetRequiredService<FinanceDbContext>());

        // Dapper
        services.AddSingleton<IDapperContext>(new DapperContext(connectionString));

        // Repositories (Dapper-based)
        services.AddScoped<InvoiceRepository>();
        services.AddScoped<BatchRepository>();
        services.AddScoped<PaymentRepository>();
        services.AddScoped<JvPostingRepository>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        // RabbitMQ
        var rabbitConfig = configuration.GetSection("RabbitMQ");
        services.AddSingleton<RabbitMqConnection>(sp =>
        {
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RabbitMqConnection>>();
            return new RabbitMqConnection(
                rabbitConfig["HostName"] ?? "localhost",
                rabbitConfig["UserName"] ?? "guest",
                rabbitConfig["Password"] ?? "guest",
                logger);
        });

        services.AddSingleton<IConnection>(sp =>
        {
            var rabbitMqConnection = sp.GetRequiredService<RabbitMqConnection>();
            return rabbitMqConnection.GetConnectionAsync().GetAwaiter().GetResult();
        });

        services.AddScoped<IMessagePublisher, MessagePublisher>();

        // RabbitMQ Consumers
        services.AddHostedService<InvoiceCreatedConsumer>();
        services.AddHostedService<PaymentProcessedConsumer>();
        services.AddHostedService<BatchApprovedConsumer>();

        // Register Infrastructure domain event handlers with MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Current User
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
