using AlertsNotifications.Application.Interfaces;
using AlertsNotifications.Domain.Interfaces;
using AlertsNotifications.Infrastructure.Messaging;
using AlertsNotifications.Infrastructure.Persistence;
using AlertsNotifications.Infrastructure.Repositories;
using AlertsNotifications.Infrastructure.Repositories.Dapper;
using AlertsNotifications.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlertsNotifications.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<AlertsNotificationsDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AlertsNotificationsDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IAlertMasterRepository, AlertMasterRepository>();
        services.AddScoped<IAlertGroupRepository, AlertGroupRepository>();
        services.AddScoped<ICircularRepository, CircularRepository>();
        services.AddScoped<ICircularSignatoryRepository, CircularSignatoryRepository>();
        services.AddScoped<ICircularTemplateRepository, CircularTemplateRepository>();
        services.AddScoped<IProbationConfirmationAlertRepository, ProbationConfirmationAlertRepository>();

        // Dapper repositories
        services.AddScoped<DapperAlertGroupRepository>();

        // RabbitMQ
        services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();
        services.AddHostedService<AlertNotificationConsumer>();
        services.AddHostedService<CircularApprovalConsumer>();

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        return services;
    }
}
