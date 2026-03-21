namespace TransactionService.Infrastructure.Messaging.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using TransactionService.Infrastructure.Messaging.Events;

public sealed class RequestCreatedConsumer : IConsumer<RequestCreatedIntegrationEvent>
{
    private readonly ILogger<RequestCreatedConsumer> _logger;

    public RequestCreatedConsumer(ILogger<RequestCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<RequestCreatedIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation(
            "Processing new stationery request: {RequestId} from Employee: {EmpId} at Location: {LocationId}",
            message.RequestId, message.RequestedBy, message.LocationId);

        // Background processing: notify approvers, send emails, etc.
        return Task.CompletedTask;
    }
}
