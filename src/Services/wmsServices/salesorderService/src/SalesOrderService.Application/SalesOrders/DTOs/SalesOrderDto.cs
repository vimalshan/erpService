namespace SalesOrderService.Application.SalesOrders.DTOs;

public record SalesOrderDto
{
    public int SoId { get; init; }
    public string SoNumber { get; init; } = "";
    public int CustomerId { get; init; }
    public int WarehouseId { get; init; }
    public DateOnly OrderDate { get; init; }
    public DateOnly? RequestedDate { get; init; }
    public string Status { get; init; } = "";
    public decimal? TotalAmount { get; init; }
    public string? Notes { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime ModifiedDate { get; init; }
    public IReadOnlyList<SalesOrderLineDto> Lines { get; init; } = [];
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
    public decimal Discount { get; init; }
    public decimal LineTotal { get; init; }
    public string? Notes { get; init; }
}

public record SalesOrderSummaryDto
{
    public int SoId { get; init; }
    public string SoNumber { get; init; } = "";
    public int CustomerId { get; init; }
    public DateOnly OrderDate { get; init; }
    public string Status { get; init; } = "";
    public decimal? TotalAmount { get; init; }
}
