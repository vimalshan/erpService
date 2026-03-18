using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskMonitor : BaseEntity
{
    public long RiskId { get; set; }
    public string MonitoredBy { get; set; } = default!;  // BRD/CLT/BLT/ULT
    public char ReviewFrequency { get; set; }  // M/H/A/Q
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
}
