namespace PurchaseOrderService.Application.DTOs;

public class PurchaseOrderLineDto
{
    public int PoLineId { get; set; }
    public int PoId { get; set; }
    public int ProductId { get; set; }
    public int LineNumber { get; set; }
    public decimal QuantityOrdered { get; set; }
    public decimal QuantityReceived { get; set; }
    public decimal? UnitPrice { get; set; }
    public string? Notes { get; set; }
    public decimal? LineTotal { get; set; }
    public bool IsFullyReceived { get; set; }
}
