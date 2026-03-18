using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskFunctionDetail : BaseEntity
{
    public long RiskId { get; set; }
    public long FunctionId { get; set; }
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }

    public RiskFunction Function { get; set; } = default!;
}
