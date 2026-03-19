using MamAllocationService.Domain.Common;

namespace MamAllocationService.Domain.Entities;

public class AllocationFg : BaseEntity
{
    public DateTime? AllDate { get; set; }
    public long? FgCode { get; set; }
    public int? DomDispatch { get; set; }
    public decimal? ExpDispatch { get; set; }
    public decimal? DutyFree { get; set; }
    public decimal? DutyPaid { get; set; }
}
