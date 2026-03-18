using MassTransit;
using Microsoft.Extensions.Logging;
using Stationery.Domain.Events;

namespace Stationery.Infrastructure.Messaging.Consumers;

public class RequestApprovedConsumer : IConsumer<RequestApprovedEvent>
{
    private readonly ILogger<RequestApprovedConsumer> _logger;

    public RequestApprovedConsumer(ILogger<RequestApprovedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<RequestApprovedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation(
            "Request approved: SubId={RequestSubId}, RequestId={RequestId}, DeptId={DeptId}, ApprovedQty={ApprovedQty}",
            message.RequestSubId, message.RequestId, message.DeptId, message.ApprovedQty);

        // Trigger notifications, update dashboards, etc.
        await Task.CompletedTask;
    }
}
