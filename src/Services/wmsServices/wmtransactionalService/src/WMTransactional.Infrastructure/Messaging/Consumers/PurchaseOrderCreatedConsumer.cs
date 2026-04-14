using MassTransit;
using Microsoft.Extensions.Logging;

namespace WMTransactional.Infrastructure.Messaging.Consumers;

public class PurchaseOrderCreatedConsumer : IConsumer<PurchaseOrderCreatedMessage>
{
    private readonly ILogger<PurchaseOrderCreatedConsumer> _logger;

    public PurchaseOrderCreatedConsumer(ILogger<PurchaseOrderCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<PurchaseOrderCreatedMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation("Purchase Order created: {PoNumber} for Supplier {SupplierId}", msg.PoNumber, msg.SupplierId);
        return Task.CompletedTask;
    }
}
