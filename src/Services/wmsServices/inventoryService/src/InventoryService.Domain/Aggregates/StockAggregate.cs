using InventoryService.Domain.Entities;
using InventoryService.Domain.Events;
using InventoryService.Domain.Exceptions;

namespace InventoryService.Domain.Aggregates;

public class StockAggregate
{
    private readonly List<InventoryTransaction> _pendingTransactions = [];

    public StockLevel StockLevel { get; }
    public IReadOnlyCollection<InventoryTransaction> PendingTransactions => _pendingTransactions.AsReadOnly();

    public StockAggregate(StockLevel stockLevel)
    {
        StockLevel = stockLevel;
    }

    public void ReceiveStock(decimal quantity, string? referenceNumber, string? createdBy)
    {
        StockLevel.ReceiveStock(quantity);

        _pendingTransactions.Add(new InventoryTransaction(
            StockLevel.ProductId, StockLevel.WarehouseId, StockLevel.BinId,
            "RECEIPT", quantity,
            referenceNumber: referenceNumber, createdBy: createdBy));
    }

    public void ShipStock(decimal quantity, string? referenceNumber, string? createdBy)
    {
        StockLevel.DeductStock(quantity);

        _pendingTransactions.Add(new InventoryTransaction(
            StockLevel.ProductId, StockLevel.WarehouseId, StockLevel.BinId,
            "SHIPMENT", -quantity,
            referenceNumber: referenceNumber, createdBy: createdBy));
    }

    public void AdjustStock(decimal newQuantity, string reason, string adjustedBy)
    {
        var diff = newQuantity - StockLevel.QuantityOnHand;
        StockLevel.AdjustQuantity(newQuantity);

        _pendingTransactions.Add(new InventoryTransaction(
            StockLevel.ProductId, StockLevel.WarehouseId, StockLevel.BinId,
            "ADJUSTMENT", diff,
            createdBy: adjustedBy, comments: reason));
    }

    public static (InventoryTransaction moveOut, InventoryTransaction moveIn) CreateTransferTransactions(
        int productId,
        int fromWarehouseId, int fromBinId,
        int toWarehouseId, int toBinId,
        decimal quantity,
        string? referenceNumber, string? createdBy)
    {
        var moveOut = new InventoryTransaction(
            productId, fromWarehouseId, fromBinId,
            "MOVE_OUT", -quantity,
            referenceNumber: referenceNumber, createdBy: createdBy);

        var moveIn = new InventoryTransaction(
            productId, toWarehouseId, toBinId,
            "MOVE_IN", quantity,
            referenceNumber: referenceNumber, createdBy: createdBy);

        return (moveOut, moveIn);
    }
}
