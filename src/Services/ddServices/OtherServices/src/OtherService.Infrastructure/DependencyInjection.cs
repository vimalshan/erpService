using Azure.Storage.Blobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OtherService.Domain.Interfaces;
using OtherService.Infrastructure.BlobStorage;
using OtherService.Infrastructure.Dapper;
using OtherService.Infrastructure.Messaging;
using OtherService.Infrastructure.Persistence;
using OtherService.Infrastructure.Repositories;

namespace OtherService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── Entity Framework ────────────────────────────────────────────────
        services.AddDbContext<OtherDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3)));

        // ── Repositories ────────────────────────────────────────────────────
        services.AddScoped<ILogDdCatDevDetailRepository, LogDdCatDevDetailRepository>();
        services.AddScoped<LogDdCatDevDetailDapperRepository>();

        // ── Blob Storage ────────────────────────────────────────────────────
        var blobConnectionString = configuration.GetConnectionString("BlobStorage");
        if (!string.IsNullOrWhiteSpace(blobConnectionString))
        {
            services.AddSingleton(_ => new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        // ── RabbitMQ / MassTransit (Optional) ─────────────────────────────────
        // Only configure if RabbitMQ is enabled (can be disabled in development)
        var rabbitMqEnabled = configuration.GetValue<bool>("Features:RabbitMQ:Enabled", false);
        if (rabbitMqEnabled)
        {
            try
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumer<LogDdCatDevDetailConsumer>();
                    x.SetKebabCaseEndpointNameFormatter();

                    x.UsingRabbitMq((ctx, cfg) =>
                    {
                        cfg.Host(configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
                        {
                            h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                            h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                        });

                        cfg.UseMessageRetry(r => r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)));

                        cfg.ReceiveEndpoint("other-service-queue", e =>
                        {
                            e.UseCircuitBreaker(cb =>
                            {
                                cb.TrackingPeriod        = TimeSpan.FromMinutes(1);
                                cb.TripThreshold         = 15;
                                cb.ActiveThreshold       = 10;
                                cb.ResetInterval         = TimeSpan.FromMinutes(5);
                            });

                            e.ConfigureConsumer<LogDdCatDevDetailConsumer>(ctx);
                        });
                    });
                });
            }
            catch (Exception ex)
            {
                // RabbitMQ optional — log and continue
                System.Diagnostics.Debug.WriteLine($"RabbitMQ initialization failed: {ex.Message}");
            }
        }

        return services;
    }
}
