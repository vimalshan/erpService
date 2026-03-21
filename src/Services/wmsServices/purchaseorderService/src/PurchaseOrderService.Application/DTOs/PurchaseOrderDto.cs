namespace PurchaseOrderService.Application.DTOs;

public class PurchaseOrderDto
{
    public int PoId { get; set; }
    public string PoNumber { get; set; } = null!;
    public int SupplierId { get; set; }
    public int WarehouseId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string Status { get; set; } = null!;
    public string? Notes { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<PurchaseOrderLineDto> Lines { get; set; } = new();
}
