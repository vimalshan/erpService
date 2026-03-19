using ProblemManagement.Domain.Common;

namespace ProblemManagement.Domain.Entities;

public class SolutionComment : BaseEntity
{
    public long SolCommentId { get; set; }
    public long SolCommentSolId { get; set; }
    public string SolCommentText { get; set; } = string.Empty;
    public long SolCommentBy { get; set; }
    public DateTime SolCommentOn { get; set; }

    // Navigation
    public ProblemSolution Solution { get; set; } = null!;
}
