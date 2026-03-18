using DeductionService.Application.CQRS.Queries.GetDeductionAmount;
using DeductionService.Application.Interfaces;
using DeductionService.Domain.Interfaces;
using DeductionService.Infrastructure.Dapper;
using DeductionService.Infrastructure.HealthChecks;
using DeductionService.Infrastructure.Messaging;
using DeductionService.Infrastructure.Messaging.Consumers;
using DeductionService.Infrastructure.Persistence;
using DeductionService.Infrastructure.Repositories;
using DeductionService.Infrastructure.Storage;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DeductionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<DeductionDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<DeductionDbContext>());

        // Repositories
        services.AddScoped<IAdhocPayDeductionRepository, AdhocPayDeductionRepository>();
        services.AddScoped<IDeductionAccessRepository, DeductionAccessRepository>();

        // Dapper
        services.AddScoped<IDeductionAmountService, DeductionDapperRepository>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
        }

        // RabbitMQ Publisher — falls back to NullMessagePublisher when broker is unavailable
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var cfg = sp.GetRequiredService<IConfiguration>();
            var pubLogger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RabbitMQPublisher>>();
            try
            {
                return RabbitMQPublisher.CreateAsync(cfg, pubLogger).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                var nullLogger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<NullMessagePublisher>>();
                nullLogger.LogWarning(ex, "[RabbitMQ] Cannot connect to broker — using NullMessagePublisher fallback.");
                return new NullMessagePublisher(nullLogger);
            }
        });

        // RabbitMQ Consumer background service
        services.AddHostedService<MonthlyDeductionConsumer>();

        // Health checks
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("database", tags: ["db", "ready"]);

        return services;
    }
}
