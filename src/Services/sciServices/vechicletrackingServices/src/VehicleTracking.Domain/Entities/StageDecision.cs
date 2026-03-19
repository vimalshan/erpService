using VehicleTracking.Domain.Common;

namespace VehicleTracking.Domain.Entities;

public class StageDecision : BaseEntity
{
    public long PurposeCode { get; set; }
    public long StageCode { get; set; }
    public string OptionName { get; set; } = string.Empty;
    public long? OptionId { get; set; }
    public long NextStage { get; set; }
}
