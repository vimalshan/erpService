using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EmailNotification.Domain.Repositories;
using EmailNotification.Infrastructure.Messaging;
using EmailNotification.Infrastructure.Resilience;
using EmailNotification.Infrastructure.Data;

namespace EmailNotification.Infrastructure;

/// <summary>
/// Extension methods for configuring infrastructure services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds infrastructure layer services to the dependency injection container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="connectionString">The database connection string</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        string connectionString)
    {
        // Register DbContext
        services.AddDbContext<Data.EmailNotificationDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sqlOptions => sqlOptions.CommandTimeout(300)));

        // Register repositories
        services.AddScoped<IEmailTypeRepository, Repositories.EmailTypeRepository>();
        services.AddScoped<IMailAccessRepository, Repositories.MailAccessRepository>();

        // Register data seeder
        services.AddScoped<IDataSeeder, EmailNotificationDataSeeder>();

        return services;
    }

    /// <summary>
    /// Adds RabbitMQ messaging services to the dependency injection container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The application configuration</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddRabbitMqServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure RabbitMQ settings
        var rabbitMqConfig = new RabbitMqConfiguration();
        configuration.GetSection("RabbitMQ").Bind(rabbitMqConfig);

        // Register RabbitMQ configuration
        services.AddSingleton(rabbitMqConfig);

        // Register RabbitMQ connection
        services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();

        // Register message consumers
        services.AddSingleton<IMessageConsumer, EmailTypeCreatedConsumer>();
        services.AddSingleton<IMessageConsumer, RecipientAddedConsumer>();

        // Register hosted service to start consumers
        services.AddHostedService<MessageConsumerHostedService>();

        return services;
    }

    /// <summary>
    /// Adds Polly resilience policies to the dependency injection container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddResiliencePolicies(this IServiceCollection services)
    {
        // Register Polly policy registry
        services.AddSingleton<IPolicyRegistry, PollyPolicyRegistry>();

        // Register resilience policy executor
        services.AddScoped<ResiliencePolicyExecutor>();

        return services;
    }
}
