using RiskService.Domain.Common;
using RiskService.Domain.Entities;

namespace RiskService.Domain.Aggregates;

public class RiskSelfAssessment : BaseEntity, IAggregateRoot
{
    public char AssessmentType { get; set; }  // O/B/U
    public long TypeReferenceId { get; set; }
    public string MonitoredBy { get; set; } = default!;  // BRD/CLT/BLT/ULT
    public DateTime DueDate { get; set; }
    public char MeetingFlag { get; set; }  // P/Y/N
    public char Status { get; set; }  // E/P/C/S
    public string? Reason { get; set; }
    public DateTime AssessmentDate { get; set; }
    public char ReviewFlag { get; set; }
    public char NewRiskFlag { get; set; }
    public string? NewRiskList { get; set; }
    public char MitigationFlag { get; set; }
    public string? MitigationList { get; set; }
    public char ApprovalStatus { get; set; }  // P/A/R
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }

    private readonly List<RiskEventAssessment> _eventAssessments = new();
    public IReadOnlyCollection<RiskEventAssessment> EventAssessments => _eventAssessments.AsReadOnly();

    private readonly List<RiskSelfAssessmentComment> _comments = new();
    public IReadOnlyCollection<RiskSelfAssessmentComment> Comments => _comments.AsReadOnly();

    public void AddEventAssessment(RiskEventAssessment ea) => _eventAssessments.Add(ea);
    public void AddComment(RiskSelfAssessmentComment comment) => _comments.Add(comment);

    public void Complete(long modifiedBy)
    {
        Status = 'C';
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void Skip(long modifiedBy, string reason)
    {
        Status = 'S';
        Reason = reason;
        MeetingFlag = 'N';
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
