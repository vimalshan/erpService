using VehicleTracking.Domain.Common;

namespace VehicleTracking.Domain.Entities;

public class WeightInformation : BaseEntity
{
    public long TrackingNumber { get; set; }
    public decimal? TyreWeight { get; set; }
    public decimal? GrossWeight { get; set; }
    public decimal? NetWeight { get; set; }
}
