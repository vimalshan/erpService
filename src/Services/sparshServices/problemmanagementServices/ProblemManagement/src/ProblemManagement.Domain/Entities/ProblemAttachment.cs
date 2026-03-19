using ProblemManagement.Domain.Common;

namespace ProblemManagement.Domain.Entities;

public class ProblemAttachment : BaseEntity
{
    public long PratId { get; set; }
    public long? PratPrId { get; set; }
    public string? PratFileName { get; set; }
    public DateTime? PratEnteredOn { get; set; }

    // Navigation
    public ProblemMain? Problem { get; set; }
}
