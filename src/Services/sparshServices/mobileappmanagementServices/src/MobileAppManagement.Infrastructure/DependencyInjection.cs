using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MobileAppManagement.Application.Interfaces;
using MobileAppManagement.Domain.Interfaces;
using MobileAppManagement.Infrastructure.BlobStorage;
using MobileAppManagement.Infrastructure.Dapper;
using MobileAppManagement.Infrastructure.Messaging;
using MobileAppManagement.Infrastructure.Persistence;
using MobileAppManagement.Infrastructure.Repositories;

namespace MobileAppManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<MobileAppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IAppDeviceRepository, AppDeviceRepository>();
        services.AddScoped<ILoginDetailRepository, LoginDetailRepository>();
        services.AddScoped<IAppRegistrationRepository, AppRegistrationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ Publisher (stub-fallback when RabbitMQ is unavailable)
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILogger<RabbitMqPublisher>>();
            try
            {
                return RabbitMqPublisher.CreateAsync(config, logger).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "RabbitMQ unavailable at startup. Using stub publisher.");
                var stubLogger = sp.GetRequiredService<ILogger<StubMessagePublisher>>();
                return new StubMessagePublisher(stubLogger);
            }
        });

        // RabbitMQ Consumers (resilient — retry every 30s if RabbitMQ is unavailable)
        services.AddHostedService<DeviceRegistrationConsumer>();
        services.AddHostedService<LoginEventConsumer>();

        return services;
    }
}
