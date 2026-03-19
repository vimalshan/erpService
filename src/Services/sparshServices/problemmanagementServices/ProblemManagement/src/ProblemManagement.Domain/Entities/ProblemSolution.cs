using ProblemManagement.Domain.Common;

namespace ProblemManagement.Domain.Entities;

public class ProblemSolution : BaseEntity
{
    public long SolId { get; set; }
    public long SolPrId { get; set; }
    public string? SolDescription { get; set; }
    public char? SolImplementation { get; set; }
    public long SolEnteredBy { get; set; }
    public DateTime SolEnteredOn { get; set; }
    public string? SolAttach { get; set; }

    // Navigation
    public ProblemMain Problem { get; set; } = null!;
    public ICollection<SolutionApproval> SolutionApprovals { get; set; } = [];
    public ICollection<SolutionComment> SolutionComments { get; set; } = [];
}
