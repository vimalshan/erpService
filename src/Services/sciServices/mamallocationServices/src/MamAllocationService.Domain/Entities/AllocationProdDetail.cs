using MamAllocationService.Domain.Common;

namespace MamAllocationService.Domain.Entities;

public class AllocationProdDetail : BaseEntity
{
    public DateTime? AllDate { get; set; }
    public long? AllSrl { get; set; }
    public int? AllFg { get; set; }
    public decimal? DdfQty { get; set; }
    public decimal? DdpQty { get; set; }
    public decimal? PrdQty { get; set; }
    public decimal? AllRm { get; set; }
}
