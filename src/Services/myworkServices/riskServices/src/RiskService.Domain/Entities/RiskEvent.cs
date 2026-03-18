using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskEvent : BaseEntity
{
    public long RiskId { get; set; }
    public string Description { get; set; } = default!;
    public DateTime EventDate { get; set; }
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
}
