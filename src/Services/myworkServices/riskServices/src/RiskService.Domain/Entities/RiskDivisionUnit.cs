using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskDivisionUnit : BaseEntity
{
    public long DivisionId { get; set; }
    public long UnitId { get; set; }

    public RiskDivision Division { get; set; } = default!;
}
