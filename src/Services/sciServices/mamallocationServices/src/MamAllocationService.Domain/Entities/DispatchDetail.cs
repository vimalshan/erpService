using MamAllocationService.Domain.Common;

namespace MamAllocationService.Domain.Entities;

public class DispatchDetail : BaseEntity
{
    public decimal? DispatchNo { get; set; }
    public DateTime? DispatchDate { get; set; }
    public int? DispatchFg { get; set; }
    public decimal? DispatchQty { get; set; }
    public string? DispatchType { get; set; }
    public DateTime? DispatchAreDate { get; set; }
    public string? DispatchInvoiceNo { get; set; }
    public long? DispatchAdvNo { get; set; }
}
