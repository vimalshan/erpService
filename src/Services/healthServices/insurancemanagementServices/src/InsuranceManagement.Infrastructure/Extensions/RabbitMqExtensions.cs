using InsuranceManagement.Infrastructure.MessageConsumers;
using InsuranceManagement.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace InsuranceManagement.Infrastructure.Extensions;

/// <summary>
/// Extension methods for RabbitMQ configuration
/// </summary>
public static class RabbitMqExtensions
{
    /// <summary>
    /// Add RabbitMQ message publisher and consumers
    /// </summary>
    public static IServiceCollection AddRabbitMqConsumers(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Get RabbitMQ configuration
        var rabbitMqConfig = new RabbitMqConfiguration();
        configuration.GetSection("RabbitMQ").Bind(rabbitMqConfig);

        // Check if RabbitMQ is enabled
        var enabled = configuration.GetValue<bool>("RabbitMQ:Enabled", true);
        if (!enabled)
        {
            return services;
        }

        // Register RabbitMQ configuration
        services.AddSingleton(rabbitMqConfig);

        // Register connection factory
        services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();

        // Register publisher
        services.AddSingleton<IInsuranceMessagePublisher, InsuranceRabbitMqPublisher>();

        // Register consumers
        services.AddSingleton<EnrollmentEventConsumer>();
        services.AddSingleton<ClaimEventConsumer>();

        // Register consumer host service
        services.AddHostedService<MessageConsumerHostedService>();

        return services;
    }
}

/// <summary>
/// Hosted service for running RabbitMQ consumers
/// </summary>
public class MessageConsumerHostedService : BackgroundService
{
    private readonly EnrollmentEventConsumer _enrollmentConsumer;
    private readonly ClaimEventConsumer _claimConsumer;
    private readonly ILogger<MessageConsumerHostedService> _logger;

    public MessageConsumerHostedService(
        EnrollmentEventConsumer enrollmentConsumer,
        ClaimEventConsumer claimConsumer,
        ILogger<MessageConsumerHostedService> logger)
    {
        _enrollmentConsumer = enrollmentConsumer ?? throw new ArgumentNullException(nameof(enrollmentConsumer));
        _claimConsumer = claimConsumer ?? throw new ArgumentNullException(nameof(claimConsumer));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Starting RabbitMQ message consumers...");

            // Start all consumers
            await Task.WhenAll(
                _enrollmentConsumer.StartAsync(stoppingToken),
                _claimConsumer.StartAsync(stoppingToken));

            _logger.LogInformation("RabbitMQ message consumers started successfully");

            // Keep the service running
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Message consumer background service is shutting down");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in background service: {ex.Message}");
            throw;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Stopping RabbitMQ message consumers...");

            await Task.WhenAll(
                _enrollmentConsumer.StopAsync(),
                _claimConsumer.StopAsync());

            _logger.LogInformation("RabbitMQ message consumers stopped");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error stopping consumers: {ex.Message}");
        }

        await base.StopAsync(cancellationToken);
    }
}
