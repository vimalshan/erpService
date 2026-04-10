using ProblemManagement.Domain.Common;

namespace ProblemManagement.Domain.Entities;

public class SolutionApproval : BaseEntity
{
    public long SolAppId { get; set; }
    public long SolAppSolId { get; set; }
    public long SolAppBy { get; set; }
    public DateTime SolAppOn { get; set; }
    public string SolAppStatus { get; set; }
    public string? SolAppReason { get; set; }
    public string? SolAppAudFlag { get; set; }

    // Navigation
    public ProblemSolution Solution { get; set; } = null!;
}
