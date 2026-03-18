using MassTransit;
using Microsoft.Extensions.Logging;
using VisitorServices.Domain.Events;

namespace VisitorServices.Infrastructure.Consumers;

public class VisitorApprovalConsumer(ILogger<VisitorApprovalConsumer> logger)
    : IConsumer<ApprovalRequestedEvent>
{
    public async Task Consume(ConsumeContext<ApprovalRequestedEvent> context)
    {
        var @event = context.Message;
        logger.LogInformation(
            "Processing approval request {RequestId} for visitor {VisitorId} — approver {ApproverId}",
            @event.ApprovalRequestId, @event.VisitorId, @event.RequiredApproverId);

        // Notify approver (e-mail / push notification integration point)
        await Task.CompletedTask;
    }
}
