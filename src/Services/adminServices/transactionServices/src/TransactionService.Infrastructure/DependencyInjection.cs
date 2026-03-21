namespace TransactionService.Infrastructure;

using Azure.Storage.Blobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionService.Application.ExternalServices;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.ExternalServices;
using TransactionService.Infrastructure.Messaging.Consumers;
using TransactionService.Infrastructure.Persistence;
using TransactionService.Infrastructure.Repositories;
using TransactionService.Infrastructure.Resilience;
using TransactionService.Infrastructure.Storage;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TransactionDb")
            ?? throw new InvalidOperationException("TransactionDb connection string not configured.");

        // EF Core
        services.AddDbContext<TransactionDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
            {
                sql.EnableRetryOnFailure(maxRetryCount: 3);
                sql.MigrationsAssembly("TransactionService.API");
                sql.CommandTimeout(30);
            }));

        // Repositories
        services.AddScoped<IRequestRepository>(sp =>
            new RequestRepository(sp.GetRequiredService<TransactionDbContext>(), connectionString));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IBudgetRepository>(sp =>
            new BudgetRepository(sp.GetRequiredService<TransactionDbContext>(), connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrWhiteSpace(blobConnectionString))
        {
            services.AddSingleton(_ => new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        // RabbitMQ via MassTransit
        var rabbitMqEnabled = !string.Equals(
            configuration["RabbitMQ:Enabled"], "false", StringComparison.OrdinalIgnoreCase);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<RequestCreatedConsumer>();
            x.AddConsumer<RequestApprovedConsumer>();
            x.AddConsumer<OrderCreatedConsumer>();
            x.AddConsumer<OrderReceivedConsumer>();

            if (rabbitMqEnabled)
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"] ?? "localhost", h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                        h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                    });

                    cfg.ReceiveEndpoint("transaction-request-created-queue", ep =>
                        ep.ConfigureConsumer<RequestCreatedConsumer>(ctx));
                    cfg.ReceiveEndpoint("transaction-request-approved-queue", ep =>
                        ep.ConfigureConsumer<RequestApprovedConsumer>(ctx));
                    cfg.ReceiveEndpoint("transaction-order-created-queue", ep =>
                        ep.ConfigureConsumer<OrderCreatedConsumer>(ctx));
                    cfg.ReceiveEndpoint("transaction-order-received-queue", ep =>
                        ep.ConfigureConsumer<OrderReceivedConsumer>(ctx));
                });
            }
            else
            {
                x.UsingInMemory((ctx, cfg) => cfg.ConfigureEndpoints(ctx));
            }
        });

        // Resilience policies (Polly)
        services.AddResiliencePolicies();

        // ── External Service HTTP Clients ──
        services.AddHttpClient<IVendorServiceClient, VendorServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ExternalServices:VendorService"] ?? "http://localhost:5003");
        }).AddResilienceHandler();

        services.AddHttpClient<ILocationServiceClient, LocationServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ExternalServices:LocationService"] ?? "http://localhost:5002");
        }).AddResilienceHandler();

        services.AddHttpClient<IFinyearServiceClient, FinyearServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ExternalServices:FinyearService"] ?? "http://localhost:5001");
        }).AddResilienceHandler();

        services.AddHttpClient<IStationeryServiceClient, StationeryServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ExternalServices:StationeryService"] ?? "http://localhost:5005");
        }).AddResilienceHandler();

        services.AddHttpClient<ILovServiceClient, LovServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ExternalServices:LovService"] ?? "http://localhost:5007");
        }).AddResilienceHandler();

        return services;
    }
}
