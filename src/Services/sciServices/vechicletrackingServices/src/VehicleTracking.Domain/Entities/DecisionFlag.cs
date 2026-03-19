using VehicleTracking.Domain.Common;

namespace VehicleTracking.Domain.Entities;

public class DecisionFlag : BaseEntity
{
    public long TrackingNumber { get; set; }
    public long PurposeCode { get; set; }
    public long StageCode { get; set; }
    public char StageDecision { get; set; }
    public char CancelFlag { get; set; }
    public long? ReferenceNumber { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string? Remark { get; set; }
}
