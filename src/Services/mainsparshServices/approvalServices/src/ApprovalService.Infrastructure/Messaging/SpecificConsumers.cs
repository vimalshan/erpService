namespace ApprovalService.Infrastructure.Messaging;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Specific consumer for Approval Master events
/// </summary>
public class ApprovalMasterEventConsumer : RabbitMqConsumerBase
{
    private readonly ILogger _logger;

    public ApprovalMasterEventConsumer(IConnection connection, ILogger<ApprovalMasterEventConsumer> logger)
        : base(connection, logger)
    {
        _logger = logger;
    }

    protected override async Task OnMessageReceivedAsync(BasicDeliverEventArgs ea)
    {
        try
        {
            var message = DecodeMessage(ea.Body.ToArray());
            _logger.LogInformation("Received Approval Master event: {Message}", message);

            // Deserialize and process the event
            var eventData = JsonSerializer.Deserialize<dynamic>(message);
            
            // Handle the event based on event type
            // - ApprovalMasterCreated
            // - ApprovalMasterUpdated
            // - ApprovalMasterStatusChanged
            // ...

            Channel?.BasicAck(ea.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing Approval Master event");
            Channel?.BasicNack(ea.DeliveryTag, false, true); // Requeue the message
        }
    }
}

/// <summary>
/// Specific consumer for Approver Employee events
/// </summary>
public class ApproverEmployeeEventConsumer : RabbitMqConsumerBase
{
    private readonly ILogger _logger;

    public ApproverEmployeeEventConsumer(IConnection connection, ILogger<ApproverEmployeeEventConsumer> logger)
        : base(connection, logger)
    {
        _logger = logger;
    }

    protected override async Task OnMessageReceivedAsync(BasicDeliverEventArgs ea)
    {
        try
        {
            var message = DecodeMessage(ea.Body.ToArray());
            _logger.LogInformation("Received Approver Employee event: {Message}", message);

            // Deserialize and process the event
            var eventData = JsonSerializer.Deserialize<dynamic>(message);
            
            // Handle the event based on event type
            // - ApproverEmployeeCreated
            // - ApproverEmployeeUpdated
            // - ApproverEmployeeStatusChanged
            // ...

            Channel?.BasicAck(ea.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing Approver Employee event");
            Channel?.BasicNack(ea.DeliveryTag, false, true); // Requeue the message
        }
    }
}

/// <summary>
/// Event consumer host for long-running consumer services
/// </summary>
public class EventConsumerHost : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventConsumerHost> _logger;

    public EventConsumerHost(IServiceProvider serviceProvider, ILogger<EventConsumerHost> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Event Consumer Host starting");

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var connection = scope.ServiceProvider.GetRequiredService<IConnection>();

            // Start Approval Master consumer
            var approvalMasterConsumer = new ApprovalMasterEventConsumer(
                connection,
                scope.ServiceProvider.GetRequiredService<ILogger<ApprovalMasterEventConsumer>>());
            approvalMasterConsumer.Start("approval-master-queue", "approval.master.*");

            // Start Approver Employee consumer
            var approverEmployeeConsumer = new ApproverEmployeeEventConsumer(
                connection,
                scope.ServiceProvider.GetRequiredService<ILogger<ApproverEmployeeEventConsumer>>());
            approverEmployeeConsumer.Start("approver-employee-queue", "approver.employee.*");

            _logger.LogInformation("Event consumers started");

            // Keep running until cancellation is requested
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Event Consumer Host stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Event Consumer Host");
            throw;
        }
    }
}
