using ProblemManagement.Domain.Common;
using ProblemManagement.Domain.Events;

namespace ProblemManagement.Domain.Entities;

public class ProblemMain : BaseEntity
{
    public long PrId { get; set; }
    public long PrOwner { get; set; }
    public long PrEnteredBy { get; set; }
    public string PrDescription { get; set; } = string.Empty;
    public DateTime? PrRespExpBy { get; set; }
    public char? PrCategory { get; set; }
    public long? PrSpecialization { get; set; }
    public string? PrImpact { get; set; }
    public string? PrExpResult { get; set; }
    public DateTime? PrEnteredOn { get; set; }
    public char PrStatus { get; set; } = 'P';
    public long? PrAppId { get; set; }
    public string? PrStatement { get; set; }
    public char? PrType { get; set; }
    public string? PrAttach { get; set; }
    public char? PrPrbFlag { get; set; }
    public string? PrPrbDescription { get; set; }
    public char? PrPostFlag { get; set; }
    public string? PrQuestion { get; set; }
    public long PrUnitId { get; set; }
    public long PrSiteId { get; set; }
    public long? PrSourceId { get; set; }
    public long PrModBy { get; set; }
    public DateTime PrModOn { get; set; }

    // Navigation properties
    public ICollection<ProblemAttachment> Attachments { get; set; } = [];
    public ICollection<ProblemSolution> Solutions { get; set; } = [];
    public ICollection<ProblemApproval> Approvals { get; set; } = [];
    public ICollection<ProblemAppAudience> Audiences { get; set; } = [];

    public static ProblemMain Create(long owner, long enteredBy, string description,
        char? category, string? impact, string? expectedResult, long unitId, long siteId)
    {
        var problem = new ProblemMain
        {
            PrOwner = owner,
            PrEnteredBy = enteredBy,
            PrDescription = description,
            PrCategory = category,
            PrImpact = impact,
            PrExpResult = expectedResult,
            PrEnteredOn = DateTime.UtcNow,
            PrStatus = 'P',
            PrUnitId = unitId,
            PrSiteId = siteId,
            PrModBy = enteredBy,
            PrModOn = DateTime.UtcNow
        };

        problem.AddDomainEvent(new ProblemCreatedEvent(problem));
        return problem;
    }

    public void Approve(long approvedBy, string? reason, char audienceFlag)
    {
        PrStatus = 'A';
        PrModBy = approvedBy;
        PrModOn = DateTime.UtcNow;
        AddDomainEvent(new ProblemApprovedEvent(this, approvedBy, reason));
    }

    public void Reject(long rejectedBy, string? reason)
    {
        PrStatus = 'R';
        PrModBy = rejectedBy;
        PrModOn = DateTime.UtcNow;
        AddDomainEvent(new ProblemRejectedEvent(this, rejectedBy, reason));
    }

    public ProblemSolution AddSolution(string? description, long enteredBy)
    {
        var solution = new ProblemSolution
        {
            SolPrId = PrId,
            SolDescription = description,
            SolEnteredBy = enteredBy,
            SolEnteredOn = DateTime.UtcNow
        };
        Solutions.Add(solution);
        AddDomainEvent(new SolutionAddedEvent(this, solution));
        return solution;
    }
}
