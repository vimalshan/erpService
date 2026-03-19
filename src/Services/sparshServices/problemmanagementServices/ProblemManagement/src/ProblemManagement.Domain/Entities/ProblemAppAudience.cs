using ProblemManagement.Domain.Common;

namespace ProblemManagement.Domain.Entities;

public class ProblemAppAudience : BaseEntity
{
    public long PrAudId { get; set; }
    public long PrAudPrId { get; set; }
    public int PrAudUnitId { get; set; }

    // Navigation
    public ProblemMain Problem { get; set; } = null!;
}
