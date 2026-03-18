using Azure.Storage.Blobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VendorService.Domain.Interfaces;
using VendorService.Infrastructure.Data;
using VendorService.Infrastructure.Messaging.Consumers;
using VendorService.Infrastructure.Repositories;
using VendorService.Infrastructure.Storage;

namespace VendorService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<DbContextOptionsBuilder>? dbContextOverride = null)
    {
        // EF Core — allow tests to substitute the database provider
        if (dbContextOverride is not null)
        {
            services.AddDbContext<VendorDbContext>(dbContextOverride);
        }
        else
        {
            services.AddDbContext<VendorDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("VendorDb"),
                    sql => sql.EnableRetryOnFailure(maxRetryCount: 3)));
        }

        // Repositories
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<ITdsVendorRepository, TdsVendorRepository>();
        services.AddScoped<ITdsFileDetailRepository, TdsFileDetailRepository>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrWhiteSpace(blobConnectionString))
        {
            services.AddSingleton(_ => new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        // RabbitMQ via MassTransit (Optional)
        var rabbitMqEnabledStr = configuration["RabbitMQ:Enabled"] ?? "true";
        var rabbitMqEnabled = !rabbitMqEnabledStr.Equals("false", StringComparison.OrdinalIgnoreCase);
        if (rabbitMqEnabled)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<VendorCreatedConsumer>();
                x.AddConsumer<VendorStatusChangedConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(
                        configuration["RabbitMQ:Host"] ?? "localhost",
                        configuration["RabbitMQ:VirtualHost"] ?? "/",
                        h =>
                        {
                            h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                            h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                        });

                    cfg.ReceiveEndpoint("vendor-created-queue", ep =>
                    {
                        ep.ConfigureConsumer<VendorCreatedConsumer>(ctx);
                    });

                    cfg.ReceiveEndpoint("vendor-status-changed-queue", ep =>
                    {
                        ep.ConfigureConsumer<VendorStatusChangedConsumer>(ctx);
                    });
                });
            });
        }
        else
        {
            // Use In-Memory transport for development/testing
            services.AddMassTransit(x =>
            {
                x.AddConsumer<VendorCreatedConsumer>();
                x.AddConsumer<VendorStatusChangedConsumer>();

                x.UsingInMemory((ctx, cfg) =>
                {
                    cfg.ReceiveEndpoint("vendor-created-queue", ep =>
                    {
                        ep.ConfigureConsumer<VendorCreatedConsumer>(ctx);
                    });

                    cfg.ReceiveEndpoint("vendor-status-changed-queue", ep =>
                    {
                        ep.ConfigureConsumer<VendorStatusChangedConsumer>(ctx);
                    });
                });
            });
        }

        return services;
    }
}
