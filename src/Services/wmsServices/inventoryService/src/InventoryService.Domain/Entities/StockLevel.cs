using InventoryService.Domain.Common;
using InventoryService.Domain.Events;
using InventoryService.Domain.Exceptions;

namespace InventoryService.Domain.Entities;

public class StockLevel : BaseEntity
{
    public long StockLevelId { get; private set; }
    public int ProductId { get; private set; }
    public int WarehouseId { get; private set; }
    public int BinId { get; private set; }
    public decimal QuantityOnHand { get; private set; }
    public decimal QuantityAllocated { get; private set; }
    public decimal QuantityReserved { get; private set; }
    public decimal QuantityAvailable => QuantityOnHand - QuantityAllocated - QuantityReserved;
    public int? ReorderLevel { get; private set; }
    public DateTime? LastCountDate { get; private set; }
    public DateTime LastUpdated { get; private set; }

    private StockLevel() { }

    public StockLevel(int productId, int warehouseId, int binId, decimal quantityOnHand)
    {
        ProductId = productId;
        WarehouseId = warehouseId;
        BinId = binId;
        QuantityOnHand = quantityOnHand;
        QuantityAllocated = 0;
        QuantityReserved = 0;
        LastUpdated = DateTime.UtcNow;

        AddDomainEvent(new StockLevelChangedEvent(productId, warehouseId, binId, 0, quantityOnHand));
    }

    public void ReceiveStock(decimal quantity)
    {
        var previousQty = QuantityOnHand;
        QuantityOnHand += quantity;
        LastUpdated = DateTime.UtcNow;
        AddDomainEvent(new StockLevelChangedEvent(ProductId, WarehouseId, BinId, previousQty, QuantityOnHand));
    }

    public void DeductStock(decimal quantity)
    {
        if (QuantityAvailable < quantity)
            throw new InsufficientStockException(ProductId, WarehouseId, BinId, quantity, QuantityAvailable);

        var previousQty = QuantityOnHand;
        QuantityOnHand -= quantity;
        LastUpdated = DateTime.UtcNow;
        AddDomainEvent(new StockLevelChangedEvent(ProductId, WarehouseId, BinId, previousQty, QuantityOnHand));
        CheckReorderLevel();
    }

    public void Allocate(decimal quantity)
    {
        if (QuantityAvailable < quantity)
            throw new InsufficientStockException(ProductId, WarehouseId, BinId, quantity, QuantityAvailable);

        QuantityAllocated += quantity;
        LastUpdated = DateTime.UtcNow;
    }

    public void Deallocate(decimal quantity)
    {
        QuantityAllocated = Math.Max(0, QuantityAllocated - quantity);
        LastUpdated = DateTime.UtcNow;
    }

    public void Reserve(decimal quantity)
    {
        if (QuantityAvailable < quantity)
            throw new InsufficientStockException(ProductId, WarehouseId, BinId, quantity, QuantityAvailable);

        QuantityReserved += quantity;
        LastUpdated = DateTime.UtcNow;
    }

    public void Unreserve(decimal quantity)
    {
        QuantityReserved = Math.Max(0, QuantityReserved - quantity);
        LastUpdated = DateTime.UtcNow;
    }

    public void AdjustQuantity(decimal newQuantity)
    {
        var previousQty = QuantityOnHand;
        QuantityOnHand = newQuantity;
        LastUpdated = DateTime.UtcNow;
        AddDomainEvent(new InventoryAdjustedEvent(ProductId, WarehouseId, BinId, previousQty, newQuantity));
        CheckReorderLevel();
    }

    public void SetReorderLevel(int? level)
    {
        ReorderLevel = level;
        LastUpdated = DateTime.UtcNow;
        CheckReorderLevel();
    }

    public void RecordCycleCount(DateTime countDate)
    {
        LastCountDate = countDate;
        LastUpdated = DateTime.UtcNow;
    }

    private void CheckReorderLevel()
    {
        if (ReorderLevel.HasValue && QuantityAvailable <= ReorderLevel.Value)
        {
            AddDomainEvent(new LowStockEvent(ProductId, WarehouseId, BinId, QuantityAvailable, ReorderLevel.Value));
        }
    }
}
