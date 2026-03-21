using MassTransit;
using Microsoft.Extensions.Logging;

namespace InventoryService.Infrastructure.Messaging.Consumers;

public class InventoryTransferConsumer : IConsumer<InventoryTransferMessage>
{
    private readonly ILogger<InventoryTransferConsumer> _logger;

    public InventoryTransferConsumer(ILogger<InventoryTransferConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<InventoryTransferMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation(
            "Inventory transfer processed: Product {ProductId}, Qty {Quantity} from Warehouse {From} to Warehouse {To}",
            msg.ProductId, msg.Quantity, msg.FromWarehouseId, msg.ToWarehouseId);
        return Task.CompletedTask;
    }
}
