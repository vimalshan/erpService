namespace InventoryService.Application.DTOs;

public record StockLevelDto
{
    public long StockLevelId { get; init; }
    public int ProductId { get; init; }
    public int WarehouseId { get; init; }
    public int BinId { get; init; }
    public decimal QuantityOnHand { get; init; }
    public decimal QuantityAllocated { get; init; }
    public decimal QuantityReserved { get; init; }
    public decimal QuantityAvailable { get; init; }
    public int? ReorderLevel { get; init; }
    public DateTime? LastCountDate { get; init; }
    public DateTime LastUpdated { get; init; }
}
