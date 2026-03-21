namespace InventoryService.Application.DTOs;

public record InventoryTransactionDto
{
    public long TransactionId { get; init; }
    public int ProductId { get; init; }
    public int WarehouseId { get; init; }
    public int? BinId { get; init; }
    public string TransactionType { get; init; } = null!;
    public decimal QuantityChange { get; init; }
    public string? ReferenceType { get; init; }
    public int? ReferenceId { get; init; }
    public string? ReferenceNumber { get; init; }
    public DateTime TransactionDate { get; init; }
    public string? CreatedBy { get; init; }
    public string? Comments { get; init; }
    public string? Notes { get; init; }
}
