using VehicleTracking.Domain.Common;

namespace VehicleTracking.Domain.Entities;

public class PurposeMaster : BaseEntity
{
    public long PurposeCode { get; set; }
    public string? PurposeName { get; set; }
    public char? TransactionType { get; set; }
    public string? PurposeCategory { get; set; }
    public long? LastStage { get; set; }
    public decimal? ParentPurpose { get; set; }

    public ICollection<PurposeStage> PurposeStages { get; set; } = [];
    public ICollection<PurposeProduct> PurposeProducts { get; set; } = [];
}
