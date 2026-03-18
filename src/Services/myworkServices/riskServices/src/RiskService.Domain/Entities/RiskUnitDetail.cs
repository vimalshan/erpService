using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskUnitDetail : BaseEntity
{
    public long RiskId { get; set; }
    public long RiskUnitId { get; set; }
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
}
