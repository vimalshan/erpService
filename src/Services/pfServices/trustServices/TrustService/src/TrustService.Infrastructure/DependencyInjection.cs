using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using TrustService.Application.Common.Interfaces;
using TrustService.Infrastructure.Messaging.Consumers;
using TrustService.Infrastructure.Persistence;
using TrustService.Infrastructure.Repositories;
using TrustService.Infrastructure.Services;

namespace TrustService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<TrustDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(TrustDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
                }));

        // Repositories
        services.AddScoped<ITrustRepository, TrustRepository>();
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<TrustDbContext>());
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Blob Storage
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        // MassTransit + RabbitMQ (or InMemory for local dev)
        services.AddMassTransit(x =>
        {
            x.AddConsumer<TrustCreatedConsumer>();
            x.AddConsumer<TrustUpdatedConsumer>();
            x.AddConsumer<TrustClosedConsumer>();
            x.AddConsumer<TrustStatusChangedConsumer>();

            var useInMemory = configuration.GetValue<bool>("RabbitMQ:UseInMemory");

            if (useInMemory)
            {
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            }
            else
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                        h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            }
        });

        // Circuit Breaker via Polly for external HTTP calls
        services.AddHttpClient("ExternalService")
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}
