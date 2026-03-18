using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskImpact : BaseEntity
{
    public long Rank { get; set; }
    public string Name { get; set; } = default!;
}
