using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskDivisionFunctionMap : BaseEntity
{
    public long DivisionId { get; set; }
    public long FunctionId { get; set; }

    public RiskDivision Division { get; set; } = default!;
    public RiskFunction Function { get; set; } = default!;
}
