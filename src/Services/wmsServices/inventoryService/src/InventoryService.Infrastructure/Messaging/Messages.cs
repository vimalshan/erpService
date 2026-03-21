using MassTransit;

namespace InventoryService.Infrastructure.Messaging;

public record StockLevelChangedMessage
{
    public int ProductId { get; init; }
    public int WarehouseId { get; init; }
    public int BinId { get; init; }
    public decimal PreviousQuantity { get; init; }
    public decimal NewQuantity { get; init; }
    public DateTime OccurredOn { get; init; }
}

public record LowStockAlertMessage
{
    public int ProductId { get; init; }
    public int WarehouseId { get; init; }
    public int BinId { get; init; }
    public decimal CurrentQuantity { get; init; }
    public int ReorderLevel { get; init; }
    public DateTime OccurredOn { get; init; }
}

public record InventoryTransferMessage
{
    public int ProductId { get; init; }
    public int FromWarehouseId { get; init; }
    public int ToWarehouseId { get; init; }
    public decimal Quantity { get; init; }
    public DateTime OccurredOn { get; init; }
}
