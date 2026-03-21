using MassTransit;
using Microsoft.Extensions.Logging;

namespace InventoryService.Infrastructure.Messaging.Consumers;

public class StockLevelChangedConsumer : IConsumer<StockLevelChangedMessage>
{
    private readonly ILogger<StockLevelChangedConsumer> _logger;

    public StockLevelChangedConsumer(ILogger<StockLevelChangedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<StockLevelChangedMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation(
            "Stock level changed for Product {ProductId} in Warehouse {WarehouseId}, Bin {BinId}: {PreviousQty} -> {NewQty}",
            msg.ProductId, msg.WarehouseId, msg.BinId, msg.PreviousQuantity, msg.NewQuantity);
        return Task.CompletedTask;
    }
}
