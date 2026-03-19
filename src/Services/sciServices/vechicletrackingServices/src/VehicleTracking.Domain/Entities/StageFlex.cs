using VehicleTracking.Domain.Common;

namespace VehicleTracking.Domain.Entities;

public class StageFlex : BaseEntity
{
    public long PurposeCode { get; set; }
    public long StageSerial { get; set; }
    public long FlexNumber { get; set; }
    public string? FlexDescription { get; set; }
    public char LovFlag { get; set; }
    public string? LovType { get; set; }
    public char? FlexType { get; set; }
}
