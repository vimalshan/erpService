namespace WMTransactional.Application.DTOs;

public record PurchaseOrderDto
{
    public int PoId { get; init; }
    public string PoNumber { get; init; } = null!;
    public int SupplierId { get; init; }
    public DateTime OrderDate { get; init; }
    public DateTime? ExpectedDate { get; init; }
    public string Status { get; init; } = null!;
    public string? Notes { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime ModifiedDate { get; init; }
    public List<PurchaseOrderLineDto> Lines { get; init; } = [];
}

public record PurchaseOrderLineDto
{
    public int PoLineId { get; init; }
    public int PoId { get; init; }
    public int ProductId { get; init; }
    public int LineNumber { get; init; }
    public decimal QuantityOrdered { get; init; }
    public decimal QuantityReceived { get; init; }
    public decimal? UnitPrice { get; init; }
    public string? Notes { get; init; }
}
