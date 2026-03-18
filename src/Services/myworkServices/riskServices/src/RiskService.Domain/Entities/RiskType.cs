using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskType : BaseEntity
{
    public string Name { get; set; } = default!;
}
