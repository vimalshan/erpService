using Azure.Storage.Blobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using VisitorServices.Application.Common.Interfaces;
using VisitorServices.Infrastructure.Consumers;
using VisitorServices.Infrastructure.Data;
using VisitorServices.Infrastructure.Repositories;
using VisitorServices.Infrastructure.Services;

namespace VisitorServices.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<VisitorDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("VisitorDb"),
                sql => sql.MigrationsAssembly(typeof(VisitorDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IVisitorRepository, VisitorRepository>();
        services.AddScoped<IVisitorItemRepository, VisitorItemRepository>();
        services.AddScoped<IApprovalRequestRepository, ApprovalRequestRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Blob Storage
        services.AddSingleton(new BlobServiceClient(
            configuration.GetConnectionString("AzureStorage") ?? "UseDevelopmentStorage=true"));
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        // MassTransit / RabbitMQ
        services.AddMassTransit(bus =>
        {
            bus.AddConsumer<VisitorApprovalConsumer>();

            bus.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(
                    configuration["RabbitMQ:Host"] ?? "localhost",
                    configuration["RabbitMQ:VirtualHost"] ?? "/",
                    h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                        h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                    });

                cfg.ConfigureEndpoints(ctx);
            });
        });

        services.AddScoped<IMessagePublisher, MessagePublisher>();

        // Polly Circuit Breaker for outbound HTTP calls
        services.AddHttpClient("VisitorServiceClient")
            .AddTransientHttpErrorPolicy(policy =>
                policy.CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30)));

        return services;
    }
}
