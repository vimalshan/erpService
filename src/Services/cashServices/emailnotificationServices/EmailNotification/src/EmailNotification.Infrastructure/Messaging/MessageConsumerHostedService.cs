using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmailNotification.Infrastructure.Messaging;

/// <summary>
/// Hosted service for running message consumers in the background
/// </summary>
public class MessageConsumerHostedService : BackgroundService
{
    private readonly IEnumerable<IMessageConsumer> _consumers;
    private readonly ILogger<MessageConsumerHostedService> _logger;

    public MessageConsumerHostedService(
        IEnumerable<IMessageConsumer> consumers,
        ILogger<MessageConsumerHostedService> logger)
    {
        _consumers = consumers;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Starting message consumers...");

            var tasks = _consumers
                .Select(consumer => consumer.StartAsync(stoppingToken))
                .ToList();

            await Task.WhenAll(tasks);

            _logger.LogInformation("All message consumers started successfully");

            // Keep the service running
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Message consumer service is shutting down");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in message consumer hosted service");
            throw;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Stopping message consumers...");

            var tasks = _consumers
                .Select(consumer => consumer.StopAsync())
                .ToList();

            await Task.WhenAll(tasks);

            _logger.LogInformation("All message consumers stopped");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error stopping message consumers");
        }

        await base.StopAsync(cancellationToken);
    }
}
