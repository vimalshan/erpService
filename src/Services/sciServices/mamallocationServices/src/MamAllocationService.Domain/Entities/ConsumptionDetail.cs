using MamAllocationService.Domain.Common;

namespace MamAllocationService.Domain.Entities;

public class ConsumptionDetail : BaseEntity
{
    public long? ConsumptionNo { get; set; }
    public DateTime? ConsumptionDate { get; set; }
    public int? ConsumptionRm { get; set; }
    public decimal? ConsumptionQty { get; set; }
}
