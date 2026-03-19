using MamAllocationService.Domain.Common;

namespace MamAllocationService.Domain.Entities;

public class FgAllocation : BaseEntity
{
    public long? Sno { get; set; }
    public long? FgCode { get; set; }
    public string? Flag { get; set; }
}
