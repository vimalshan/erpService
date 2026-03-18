using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskFrequencyMap : BaseEntity
{
    public long RatingId { get; set; }
    public string MonitorCode { get; set; } = default!;  // BRD/CLT/BLT/ULT
    public char FrequencyCode { get; set; }  // M/H/A/Q
    public string ReviewMonth { get; set; } = default!;
    public int ReviewDay { get; set; }
}
