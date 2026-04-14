namespace WMTransactional.Application.DTOs;

public record SalesOrderDto
{
    public int SoId { get; init; }
    public string SoNumber { get; init; } = null!;
    public int CustomerId { get; init; }
    public DateTime OrderDate { get; init; }
    public DateTime? RequestedDate { get; init; }
    public string Status { get; init; } = null!;
    public string? Notes { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime ModifiedDate { get; init; }
    public List<SalesOrderLineDto> Lines { get; init; } = [];
}

public record SalesOrderLineDto
{
    public int SoLineId { get; init; }
    public int SoId { get; init; }
    public int ProductId { get; init; }
    public int LineNumber { get; init; }
    public decimal QuantityOrdered { get; init; }
    public decimal QuantityShipped { get; init; }
    public decimal? UnitPrice { get; init; }
    public string? Notes { get; init; }
}
