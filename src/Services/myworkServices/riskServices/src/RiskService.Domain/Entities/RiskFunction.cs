using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskFunction : BaseEntity
{
    public string Name { get; set; } = default!;
}
