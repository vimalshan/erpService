using MamAllocationService.Domain.Common;

namespace MamAllocationService.Domain.Entities;

public class ProductAllocation : BaseEntity
{
    public long? Sno { get; set; }
    public long? RmCode { get; set; }
}
