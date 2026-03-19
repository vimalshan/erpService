using VehicleTracking.Domain.Common;

namespace VehicleTracking.Domain.Entities;

public class PurposeStage : BaseEntity
{
    public long PurposeCode { get; set; }
    public long StageCode { get; set; }
    public long StageSerial { get; set; }
    public char FlexField { get; set; }
    public char ParallelFlag { get; set; }
    public long RoleCode { get; set; }
    public char BooleanFlag { get; set; }
    public string? BooleanDescription { get; set; }
    public long? TrueStage { get; set; }
    public long? FalseStage { get; set; }
    public string? Remarks { get; set; }
    public decimal? LowLimit { get; set; }
    public decimal? HighLimit { get; set; }
    public decimal? TargetTime { get; set; }

    public PurposeMaster? Purpose { get; set; }
    public StageMaster? Stage { get; set; }
}
