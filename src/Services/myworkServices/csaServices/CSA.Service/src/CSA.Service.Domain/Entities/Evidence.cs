using CSA.Service.Domain.Common;

namespace CSA.Service.Domain.Entities;

public class Evidence : BaseEntity
{
    public long EvidenceId { get; set; }
    public long ControlId { get; set; }
    public string? Name { get; set; }
    public string? TempName { get; set; }

    // Navigation
    public Control? Control { get; set; }
    public ICollection<SurveyAttachment> SurveyAttachments { get; set; } = [];
}
