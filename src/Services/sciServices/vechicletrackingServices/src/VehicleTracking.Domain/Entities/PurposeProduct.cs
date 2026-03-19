using VehicleTracking.Domain.Common;

namespace VehicleTracking.Domain.Entities;

public class PurposeProduct : BaseEntity
{
    public string ProductCode { get; set; } = string.Empty;
    public long PurposeCode { get; set; }

    public PurposeMaster? Purpose { get; set; }
}
