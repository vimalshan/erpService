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

        // RabbitMQ Publisher (lazy initialization to avoid blocking on startup)
        // Registered as singleton factory, actual async init deferred to first usage
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RabbitMqPublisher>>();
            // For production, use lazy initialization or background startup
            // For now, catch connection errors to prevent startup failure
            try
            {
                return RabbitMqPublisher.CreateAsync(config, logger).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to connect to RabbitMQ at startup. Using stub publisher.");
                var stubLogger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<StubMessagePublisher>>();
                return new StubMessagePublisher(stubLogger);
            }
        });

        // RabbitMQ Consumers (commented out for testing without RabbitMQ)
        // TODO: Enable when RabbitMQ is available in your environment
        // services.AddHostedService<DeviceRegistrationConsumer>();
        // services.AddHostedService<LoginEventConsumer>();

        return services;
    }
}
