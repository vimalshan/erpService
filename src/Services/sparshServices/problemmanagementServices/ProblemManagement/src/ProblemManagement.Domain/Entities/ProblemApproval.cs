using ProblemManagement.Domain.Common;

namespace ProblemManagement.Domain.Entities;

public class ProblemApproval : BaseEntity
{
    public long PrAppId { get; set; }
    public long PrAppPrId { get; set; }
    public long PrAppBy { get; set; }
    public DateTime PrAppOn { get; set; }
    public string PrAppStatus { get; set; }
    public string? PrAppReason { get; set; }
    public string PrAppAudFlag { get; set; }

    // Navigation
    public ProblemMain Problem { get; set; } = null!;
}
