using MamAllocationService.Domain.Common;

namespace MamAllocationService.Domain.Entities;

public class ArrivalDetail : BaseEntity
{
    public long? ArrivalNo { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public decimal? ArrivalQty { get; set; }
    public int? ArrivalItem { get; set; }
    public decimal? ArrivalReceiptNo { get; set; }
}
