using MassTransit;
using Microsoft.Extensions.Logging;
using InventoryService.Domain.Interfaces;

namespace InventoryService.Infrastructure.Messaging.Consumers;

public class LowStockAlertConsumer : IConsumer<LowStockAlertMessage>
{
    private readonly ILogger<LowStockAlertConsumer> _logger;

    public LowStockAlertConsumer(ILogger<LowStockAlertConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<LowStockAlertMessage> context)
    {
        var msg = context.Message;
        _logger.LogWarning(
            "LOW STOCK ALERT: Product {ProductId} in Warehouse {WarehouseId}, Bin {BinId}. Current: {CurrentQty}, Reorder Level: {ReorderLevel}",
            msg.ProductId, msg.WarehouseId, msg.BinId, msg.CurrentQuantity, msg.ReorderLevel);
        return Task.CompletedTask;
    }
}
