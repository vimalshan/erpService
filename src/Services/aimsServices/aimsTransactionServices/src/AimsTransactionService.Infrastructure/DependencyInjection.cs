using Azure.Storage.Blobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Infrastructure.Consumers;
using AimsTransactionService.Infrastructure.Data;
using AimsTransactionService.Infrastructure.Repositories;
using AimsTransactionService.Infrastructure.Services;

namespace AimsTransactionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<AimsTransactionDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("AimsTransactionDb"),
                sql => sql.MigrationsAssembly(typeof(AimsTransactionDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ISwipeRepository, SwipeRepository>();
        services.AddScoped<IAttendanceBatchRepository, AttendanceBatchRepository>();
        services.AddScoped<ILeaveRepository, LeaveRepository>();
        services.AddScoped<ICompOffRepository, CompOffRepository>();
        services.AddScoped<IAttendanceSummaryRepository, AttendanceSummaryRepository>();
        services.AddScoped<ILeaveCreditRepository, LeaveCreditRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper Read Service
        services.AddScoped<AimsReadService>();

        // Blob Storage
        var azureStorageConn = configuration.GetConnectionString("AzureStorage");
        if (!string.IsNullOrWhiteSpace(azureStorageConn))
        {
            services.AddSingleton(new BlobServiceClient(azureStorageConn));
        }
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        // MassTransit / RabbitMQ
        services.AddMassTransit(bus =>
        {
            bus.AddConsumer<SwipeProcessingConsumer>();
            bus.AddConsumer<LeaveApprovalConsumer>();

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
        services.AddHttpClient("AimsTransactionServiceClient")
            .AddTransientHttpErrorPolicy(policy =>
                policy.CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30)));

        return services;
    }
}
