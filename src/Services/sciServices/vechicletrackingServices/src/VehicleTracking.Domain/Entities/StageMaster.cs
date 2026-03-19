using VehicleTracking.Domain.Common;

namespace VehicleTracking.Domain.Entities;

public class StageMaster : AuditableEntity
{
    public long StageCode { get; set; }
    public string OptionName { get; set; } = string.Empty;

    public ICollection<PurposeStage> PurposeStages { get; set; } = [];
}
