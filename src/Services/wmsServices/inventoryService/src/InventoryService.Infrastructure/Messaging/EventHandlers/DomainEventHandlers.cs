using MediatR;
using MassTransit;
using InventoryService.Domain.Events;
using InventoryService.Infrastructure.Messaging;

namespace InventoryService.Infrastructure.Messaging.EventHandlers;

public class StockLevelChangedEventHandler : INotificationHandler<StockLevelChangedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public StockLevelChangedEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(StockLevelChangedEvent notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new StockLevelChangedMessage
        {
            ProductId = notification.ProductId,
            WarehouseId = notification.WarehouseId,
            BinId = notification.BinId,
            PreviousQuantity = notification.PreviousQuantity,
            NewQuantity = notification.NewQuantity,
            OccurredOn = notification.OccurredOn
        }, cancellationToken);
    }
}

public class LowStockEventHandler : INotificationHandler<LowStockEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public LowStockEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(LowStockEvent notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new LowStockAlertMessage
        {
            ProductId = notification.ProductId,
            WarehouseId = notification.WarehouseId,
            BinId = notification.BinId,
            CurrentQuantity = notification.CurrentQuantity,
            ReorderLevel = notification.ReorderLevel,
            OccurredOn = notification.OccurredOn
        }, cancellationToken);
    }
}

public class InventoryTransferredEventHandler : INotificationHandler<InventoryTransferredEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public InventoryTransferredEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(InventoryTransferredEvent notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new InventoryTransferMessage
        {
            ProductId = notification.ProductId,
            FromWarehouseId = notification.FromWarehouseId,
            ToWarehouseId = notification.ToWarehouseId,
            Quantity = notification.Quantity,
            OccurredOn = notification.OccurredOn
        }, cancellationToken);
    }
}
