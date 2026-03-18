using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskCause : BaseEntity
{
    public long RiskId { get; set; }
    public string Description { get; set; } = default!;
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
}
