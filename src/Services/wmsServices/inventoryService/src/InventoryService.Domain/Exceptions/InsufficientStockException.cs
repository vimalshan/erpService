namespace InventoryService.Domain.Exceptions;

public class InsufficientStockException : Exception
{
    public int ProductId { get; }
    public int WarehouseId { get; }
    public int BinId { get; }
    public decimal RequestedQuantity { get; }
    public decimal AvailableQuantity { get; }

    public InsufficientStockException(int productId, int warehouseId, int binId, decimal requested, decimal available)
        : base($"Insufficient stock for product {productId} in warehouse {warehouseId}, bin {binId}. Requested: {requested}, Available: {available}")
    {
        ProductId = productId;
        WarehouseId = warehouseId;
        BinId = binId;
        RequestedQuantity = requested;
        AvailableQuantity = available;
    }
}
