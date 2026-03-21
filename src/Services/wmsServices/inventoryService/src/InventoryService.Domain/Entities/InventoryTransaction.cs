using InventoryService.Domain.Common;
using InventoryService.Domain.ValueObjects;

namespace InventoryService.Domain.Entities;

public class InventoryTransaction : BaseEntity
{
    public long TransactionId { get; private set; }
    public int ProductId { get; private set; }
    public int WarehouseId { get; private set; }
    public int? BinId { get; private set; }
    public string TransactionType { get; private set; } = null!;
    public decimal QuantityChange { get; private set; }
    public string? ReferenceType { get; private set; }
    public int? ReferenceId { get; private set; }
    public string? ReferenceNumber { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public string? CreatedBy { get; private set; }
    public string? Comments { get; private set; }
    public string? Notes { get; private set; }

    private InventoryTransaction() { }

    public InventoryTransaction(
        int productId,
        int warehouseId,
        int? binId,
        string transactionType,
        decimal quantityChange,
        string? referenceType = null,
        int? referenceId = null,
        string? referenceNumber = null,
        string? createdBy = null,
        string? comments = null,
        string? notes = null)
    {
        // Validate transaction type
        ValueObjects.TransactionType.From(transactionType);

        ProductId = productId;
        WarehouseId = warehouseId;
        BinId = binId;
        TransactionType = transactionType;
        QuantityChange = quantityChange;
        ReferenceType = referenceType;
        ReferenceId = referenceId;
        ReferenceNumber = referenceNumber;
        TransactionDate = DateTime.UtcNow;
        CreatedBy = createdBy;
        Comments = comments;
        Notes = notes;
    }
}
