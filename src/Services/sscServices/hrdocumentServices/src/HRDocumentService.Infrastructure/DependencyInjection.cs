using HRDocumentService.Application.Interfaces;
using HRDocumentService.Domain.Interfaces;
using HRDocumentService.Infrastructure.BlobStorage;
using HRDocumentService.Infrastructure.Dapper;
using HRDocumentService.Infrastructure.Messaging;
using HRDocumentService.Infrastructure.Persistence;
using HRDocumentService.Infrastructure.Repositories;
using HRDocumentService.Infrastructure.Resilience;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HRDocumentService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<HRDocumentDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(HRDocumentDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IHRDocumentRepository, HRDocumentRepository>();
        services.AddScoped<IHRDocumentFileRepository, HRDocumentFileRepository>();
        services.AddScoped<IHRDocumentReceiptRepository, HRDocumentReceiptRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ - gracefully fallback if unavailable
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            try
            {
                var logger = sp.GetRequiredService<ILogger<RabbitMQPublisher>>();
                return RabbitMQPublisher.CreateAsync(config, logger).GetAwaiter().GetResult();
            }
            catch
            {
                var fallbackLogger = sp.GetRequiredService<ILogger<NullMessagePublisher>>();
                fallbackLogger.LogWarning("RabbitMQ is unavailable. Using NullMessagePublisher fallback.");
                return new NullMessagePublisher(fallbackLogger);
            }
        });
        services.AddHostedService<DocumentApprovalConsumer>();

        // Polly Resilience
        services.AddResiliencePolicies();

        return services;
    }
}
