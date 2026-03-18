using DealTicketing.Application.Common.Interfaces;
using DealTicketing.Domain.Interfaces;
using DealTicketing.Infrastructure.BlobStorage;
using DealTicketing.Infrastructure.Messaging;
using DealTicketing.Infrastructure.Persistence;
using DealTicketing.Infrastructure.ReadRepositories;
using DealTicketing.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace DealTicketing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // ── EF Core ─────────────────────────────────────────────────────────
        services.AddDbContext<DealTicketingDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DealTicketingDb"),
                sql => sql.MigrationsAssembly(typeof(DealTicketingDbContext).Assembly.FullName)
                          .EnableRetryOnFailure(3)));

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<DealTicketingDbContext>());

        // ── Repositories ────────────────────────────────────────────────────
        services.AddScoped<IDealBatchRepository, DealBatchRepository>();
        services.AddScoped<IDealDetailRepository, DealDetailRepository>();
        services.AddScoped<IDealSettlementRepository, DealSettlementRepository>();
        services.AddScoped<IBankRepository, BankRepository>();

        // ── Dapper Read Repositories ─────────────────────────────────────────
        services.AddScoped<DealDapperReadRepository>();

        // ── Blob Storage ─────────────────────────────────────────────────────
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // ── MassTransit / RabbitMQ ───────────────────────────────────────────
        var rabbitMqHost = configuration["RabbitMQ:Host"];
        var useRabbitMq = !string.IsNullOrWhiteSpace(rabbitMqHost) && rabbitMqHost != "disabled";

        services.AddMassTransit(x =>
        {
            x.AddConsumer<DealBatchCreatedConsumer>();
            x.AddConsumer<DealApprovedConsumer>();
            x.AddConsumer<DealRejectedConsumer>();
            x.AddConsumer<DealSettledConsumer>();

            if (useRabbitMq)
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbitMqHost, "/", h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                        h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                    });

                    cfg.ReceiveEndpoint("deal-batch-created", ep =>
                        ep.ConfigureConsumer<DealBatchCreatedConsumer>(ctx));
                    cfg.ReceiveEndpoint("deal-approved", ep =>
                        ep.ConfigureConsumer<DealApprovedConsumer>(ctx));
                    cfg.ReceiveEndpoint("deal-rejected", ep =>
                        ep.ConfigureConsumer<DealRejectedConsumer>(ctx));
                    cfg.ReceiveEndpoint("deal-settled", ep =>
                        ep.ConfigureConsumer<DealSettledConsumer>(ctx));

                    cfg.ConfigureEndpoints(ctx);
                });
            }
            else
            {
                // InMemory transport for local development (no RabbitMQ required)
                x.UsingInMemory((ctx, cfg) =>
                {
                    cfg.ConfigureEndpoints(ctx);
                });
            }
        });

        // ── Polly Circuit Breaker ─────────────────────────────────────────────
        services.AddHttpClient("ExternalRates")
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}
