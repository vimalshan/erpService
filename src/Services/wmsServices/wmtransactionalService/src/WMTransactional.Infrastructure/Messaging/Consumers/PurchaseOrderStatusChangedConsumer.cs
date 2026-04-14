using MassTransit;
using Microsoft.Extensions.Logging;

namespace WMTransactional.Infrastructure.Messaging.Consumers;

public class PurchaseOrderStatusChangedConsumer : IConsumer<PurchaseOrderStatusChangedMessage>
{
    private readonly ILogger<PurchaseOrderStatusChangedConsumer> _logger;

    public PurchaseOrderStatusChangedConsumer(ILogger<PurchaseOrderStatusChangedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<PurchaseOrderStatusChangedMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation("Purchase Order {PoNumber} status changed to {NewStatus}",
            msg.PoNumber, msg.NewStatus);
        return Task.CompletedTask;
    }
}
