using MassTransit;
using Microsoft.Extensions.Logging;
using Stationery.Domain.Events;

namespace Stationery.Infrastructure.Messaging.Consumers;

public class StockLevelChangedConsumer : IConsumer<StockLevelChangedEvent>
{
    private readonly ILogger<StockLevelChangedConsumer> _logger;

    public StockLevelChangedConsumer(ILogger<StockLevelChangedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<StockLevelChangedEvent> context)
    {
        var message = context.Message;

        if (message.IsBelowReorderLevel)
        {
            _logger.LogWarning(
                "LOW STOCK: Item {StationaryId} ({Description}) - Stock: {NewStock}, Reorder Level: {ReorderLevel}. Auto-reorder may be required.",
                message.StationaryId, message.Description, message.NewStock, message.ReorderLevel);
        }
        else
        {
            _logger.LogInformation(
                "Stock updated for Item {StationaryId} ({Description}) - New Stock: {NewStock}",
                message.StationaryId, message.Description, message.NewStock);
        }

        await Task.CompletedTask;
    }
}
