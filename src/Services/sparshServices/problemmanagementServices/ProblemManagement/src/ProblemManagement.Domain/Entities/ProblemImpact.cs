using ProblemManagement.Domain.Common;

namespace ProblemManagement.Domain.Entities;

public class ProblemImpact : BaseEntity
{
    public long ImpactId { get; set; }
    public string ImpactDesc { get; set; } = string.Empty;
}
