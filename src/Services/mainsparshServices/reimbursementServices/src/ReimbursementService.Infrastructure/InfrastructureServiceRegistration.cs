using Azure.Storage.Blobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using ReimbursementService.Domain.Interfaces;
using ReimbursementService.Infrastructure.BlobStorage;
using ReimbursementService.Infrastructure.Dapper;
using ReimbursementService.Infrastructure.Messaging;
using ReimbursementService.Infrastructure.Messaging.Consumers;
using ReimbursementService.Infrastructure.Messaging.Contracts;
using ReimbursementService.Infrastructure.Persistence;
using ReimbursementService.Infrastructure.Persistence.Repositories;

namespace ReimbursementService.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── EF Core ─────────────────────────────────────────────────────────────
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(maxRetryCount: 3)));

        // ── Repositories ────────────────────────────────────────────────────────
        services.AddScoped<IReimbursementRepository, ReimbursementRepository>();
        services.AddScoped<DapperReimbursementReadService>();

        // ── Event Publisher ─────────────────────────────────────────────────────
        services.AddScoped<IEventPublisher, MassTransitEventPublisher>();

        // ── MassTransit ──────────────────────────────────────────────────────────
        // Defaults to in-memory transport for local development.
        // Set RabbitMQ:UseInMemory=false in appsettings to use a real broker.
        var useInMemory = configuration.GetValue("RabbitMQ:UseInMemory", true);

        services.AddMassTransit(bus =>
        {
            bus.AddConsumer<ReimbursementSubmittedConsumer>();
            bus.AddConsumer<ReimbursementApprovedConsumer>();
            bus.AddConsumer<ReimbursementPaidConsumer>();

            if (useInMemory)
            {
                bus.UsingInMemory((ctx, cfg) =>
                {
                    cfg.ConfigureEndpoints(ctx);
                });
            }
            else
            {
                bus.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                        h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                    });

                    cfg.ReceiveEndpoint("reimbursement-submitted", ep =>
                        ep.ConfigureConsumer<ReimbursementSubmittedConsumer>(ctx));
                    cfg.ReceiveEndpoint("reimbursement-approved", ep =>
                        ep.ConfigureConsumer<ReimbursementApprovedConsumer>(ctx));
                    cfg.ReceiveEndpoint("reimbursement-paid", ep =>
                        ep.ConfigureConsumer<ReimbursementPaidConsumer>(ctx));
                });
            }
        });

        // ── Azure Blob Storage ───────────────────────────────────────────────────
        var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrWhiteSpace(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
        }

        // ── Polly Circuit Breaker (registered as a named HttpClient policy) ─────
        services.AddHttpClient("ReimbursementClient")
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30));
}
