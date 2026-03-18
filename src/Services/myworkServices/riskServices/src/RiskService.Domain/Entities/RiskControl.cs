using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskControl : BaseEntity
{
    public long RiskId { get; set; }
    public string Description { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
    public long? ImpactReductionPercent { get; set; }
    public long? ProbabilityReductionPercent { get; set; }
}
