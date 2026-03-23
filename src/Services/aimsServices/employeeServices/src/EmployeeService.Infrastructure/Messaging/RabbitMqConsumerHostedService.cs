using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmployeeService.Infrastructure.Messaging;

/// <summary>
/// Hosted service that starts the RabbitMQ consumers when the application starts.
/// Consumers are resolved via DI (singletons) and their StartAsync is called here.
/// </summary>
public sealed class RabbitMqConsumerHostedService : BackgroundService
{
    private readonly AttendanceFlagConsumer _attendanceFlagConsumer;
    private readonly ApproverAssignmentConsumer _approverAssignmentConsumer;
    private readonly ILogger<RabbitMqConsumerHostedService> _logger;

    public RabbitMqConsumerHostedService(
        AttendanceFlagConsumer attendanceFlagConsumer,
        ApproverAssignmentConsumer approverAssignmentConsumer,
        ILogger<RabbitMqConsumerHostedService> logger)
    {
        _attendanceFlagConsumer   = attendanceFlagConsumer;
        _approverAssignmentConsumer = approverAssignmentConsumer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Employee RabbitMQ Consumer Hosted Service starting");

        try
        {
            // Start both consumers concurrently; each blocks internally while consuming
            await Task.WhenAll(
                _attendanceFlagConsumer.StartAsync(stoppingToken),
                _approverAssignmentConsumer.StartAsync(stoppingToken)
            );

            // Keep alive until cancellation
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Employee RabbitMQ Consumer Hosted Service stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error in Employee RabbitMQ Consumer Hosted Service");
        }
    }
}
