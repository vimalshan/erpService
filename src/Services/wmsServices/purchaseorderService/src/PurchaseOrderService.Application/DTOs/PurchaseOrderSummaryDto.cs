namespace PurchaseOrderService.Application.DTOs;

public class PurchaseOrderSummaryDto
{
    public int PoId { get; set; }
    public string PoNumber { get; set; } = null!;
    public int SupplierId { get; set; }
    public int WarehouseId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = null!;
    public decimal? TotalAmount { get; set; }
    public int LineCount { get; set; }
}
