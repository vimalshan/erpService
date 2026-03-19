using VehicleTracking.Domain.Common;

namespace VehicleTracking.Domain.Entities;

public class VehicleDirectEntry : BaseEntity
{
    public long Id { get; set; }
    public long TrackingNumber { get; set; }
    public DateTime EntryDate { get; set; }
    public string EntryUser { get; set; } = string.Empty;
}
