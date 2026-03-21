namespace TransactionService.Infrastructure.Messaging.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using TransactionService.Infrastructure.Messaging.Events;

public sealed class RequestApprovedConsumer : IConsumer<RequestApprovedIntegrationEvent>
{
    private readonly ILogger<RequestApprovedConsumer> _logger;

    public RequestApprovedConsumer(ILogger<RequestApprovedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<RequestApprovedIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation(
            "Request {RequestSubId} approved with qty {ApprovedQty} by Approver: {ApproverSysId}",
            message.RequestSubId, message.ApprovedQty, message.ApproverSysId);

        // Background: update dashboards, notify requestor, trigger procurement
        return Task.CompletedTask;
    }
}
