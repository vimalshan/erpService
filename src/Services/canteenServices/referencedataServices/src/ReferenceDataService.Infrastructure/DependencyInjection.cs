using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReferenceDataService.Application.Interfaces;
using ReferenceDataService.Domain.Interfaces;
using ReferenceDataService.Infrastructure.Dapper;
using ReferenceDataService.Infrastructure.Messaging;
using ReferenceDataService.Infrastructure.Messaging.Consumers;
using ReferenceDataService.Infrastructure.Persistence;
using ReferenceDataService.Infrastructure.Repositories;
using ReferenceDataService.Infrastructure.Services;

namespace ReferenceDataService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ReferenceDataDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ReferenceDataDbContext).Assembly.FullName)));

        // Dapper
        services.AddSingleton<DapperContext>();
        services.AddScoped<DapperRepository>();

        // Repositories
        services.AddScoped<ILovMasterRepository, LovMasterRepository>();
        services.AddScoped<ILovTypeMasterRepository, LovTypeMasterRepository>();
        services.AddScoped<IPathToSqlServerRepository, PathToSqlServerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Azure Blob Storage
        var blobConnectionString = configuration["AzureBlobStorage:ConnectionString"];
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        // RabbitMQ
        services.AddSingleton<RabbitMqConnection>();
        services.AddScoped<IMessagePublisher, MessagePublisher>();

        // Message Consumers
        services.AddHostedService<LovMasterCreatedConsumer>();
        services.AddHostedService<LovTypeMasterCreatedConsumer>();

        // Health Checks
        services.AddHealthChecks()
            .AddDbContextCheck<ReferenceDataDbContext>("database");

        return services;
    }
}
