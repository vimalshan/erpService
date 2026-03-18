using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskProbability : BaseEntity
{
    public long Rank { get; set; }
    public string Name { get; set; } = default!;
    public string Occurrence { get; set; } = default!;
}
