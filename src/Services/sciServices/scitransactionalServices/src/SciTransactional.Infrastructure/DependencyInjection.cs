using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SciTransactional.Application.Interfaces;
using SciTransactional.Domain.Interfaces;
using SciTransactional.Infrastructure.Dapper;
using SciTransactional.Infrastructure.Messaging;
using SciTransactional.Infrastructure.Persistence;
using SciTransactional.Infrastructure.Repositories;
using SciTransactional.Infrastructure.Services;

namespace SciTransactional.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<SciTransactionalDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<INavigationRepository, NavigationRepository>();
        services.AddScoped<INormsRepository, NormsRepository>();
        services.AddScoped<IAdvanceLicenseRepository, AdvanceLicenseRepository>();
        services.AddScoped<IAutoMailRepository, AutoMailRepository>();
        services.AddScoped<IOrderMapRepository, OrderMapRepository>();
        services.AddScoped<IDirectEntryRepository, DirectEntryRepository>();

        // Dapper
        services.AddSingleton<IDapperContext, DapperContext>();

        // Blob Storage
        var blobConnectionString = configuration["AzureBlobStorage:ConnectionString"];
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddSingleton<IBlobStorageService, BlobStorageService>();
        }
        else
        {
            services.AddSingleton<IBlobStorageService, NoOpBlobStorageService>();
        }

        // RabbitMQ
        var rabbitHost = configuration["RabbitMQ:HostName"];
        if (!string.IsNullOrEmpty(rabbitHost))
        {
            services.AddSingleton<IRabbitMqPublisher>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var logger = sp.GetRequiredService<ILogger<RabbitMqPublisher>>();
                try
                {
                    return RabbitMqPublisher.CreateAsync(config, logger).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "RabbitMQ is unavailable. Using no-op publisher.");
                    return new NoOpRabbitMqPublisher();
                }
            });

            services.AddHostedService<NavigationUpdatedConsumer>();
        }
        else
        {
            services.AddSingleton<IRabbitMqPublisher, NoOpRabbitMqPublisher>();
        }

        return services;
    }
}
