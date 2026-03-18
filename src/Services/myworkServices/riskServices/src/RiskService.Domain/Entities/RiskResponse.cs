using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskResponse : BaseEntity
{
    public string Name { get; set; } = default!;
}
