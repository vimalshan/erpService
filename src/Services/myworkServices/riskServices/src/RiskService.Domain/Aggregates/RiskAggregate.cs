using RiskService.Domain.Common;
using RiskService.Domain.Entities;
using RiskService.Domain.Events;

namespace RiskService.Domain.Aggregates;

public class RiskAggregate : BaseEntity, IAggregateRoot
{
    public char ApplicableTo { get; set; }  // O/B/S/U
    public long OrganizationId { get; set; }
    public long BusinessId { get; set; }
    public long DivisionId { get; set; }
    public long UnitId { get; set; }
    public long FunctionId { get; set; }
    public string EventTitle { get; set; } = default!;
    public string Description { get; set; } = default!;
    public long TypeId { get; set; }

    // Inherent Risk Rating
    public long ImpactId { get; set; }
    public long ProbabilityId { get; set; }
    public long RatingId { get; set; }

    // Residual Risk Rating (after controls)
    public long ResidualImpactId { get; set; }
    public long ResidualProbabilityId { get; set; }
    public long ResidualRatingId { get; set; }

    public long ResponseId { get; set; }
    public char MitigationFlag { get; set; }  // Y/N
    public long OwnerId { get; set; }
    public char ApprovalStatus { get; set; }  // E/P/A
    public DateTime? CancelDate { get; set; }
    public string? CancelReason { get; set; }
    public long? AssessmentId { get; set; }

    // Reviewed ratings
    public long? ReviewedImpactId { get; set; }
    public long? ReviewedProbabilityId { get; set; }
    public long? ReviewedRiskRatingId { get; set; }

    // Navigation properties
    public RiskType Type { get; set; } = default!;
    public RiskImpact Impact { get; set; } = default!;
    public RiskProbability Probability { get; set; } = default!;
    public RiskRating Rating { get; set; } = default!;
    public RiskResponse Response { get; set; } = default!;

    private readonly List<RiskCause> _causes = new();
    public IReadOnlyCollection<RiskCause> Causes => _causes.AsReadOnly();

    private readonly List<RiskControl> _controls = new();
    public IReadOnlyCollection<RiskControl> Controls => _controls.AsReadOnly();

    private readonly List<RiskImpactMap> _impactMaps = new();
    public IReadOnlyCollection<RiskImpactMap> ImpactMaps => _impactMaps.AsReadOnly();

    private readonly List<RiskEvent> _events = new();
    public IReadOnlyCollection<RiskEvent> Events => _events.AsReadOnly();

    private readonly List<RiskMonitor> _monitors = new();
    public IReadOnlyCollection<RiskMonitor> Monitors => _monitors.AsReadOnly();

    private readonly List<RiskFunctionDetail> _functionDetails = new();
    public IReadOnlyCollection<RiskFunctionDetail> FunctionDetails => _functionDetails.AsReadOnly();

    private readonly List<RiskUnitDetail> _unitDetails = new();
    public IReadOnlyCollection<RiskUnitDetail> UnitDetails => _unitDetails.AsReadOnly();

    private readonly List<RiskApproval> _approvals = new();
    public IReadOnlyCollection<RiskApproval> Approvals => _approvals.AsReadOnly();

    private readonly List<RiskMitigation> _mitigations = new();
    public IReadOnlyCollection<RiskMitigation> Mitigations => _mitigations.AsReadOnly();

    // Domain methods
    public void Submit(long submittedBy)
    {
        ApprovalStatus = 'P';
        ModifiedBy = submittedBy;
        ModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new RiskSubmittedEvent(Id, submittedBy));
    }

    public void Approve(long approverId, string remarks)
    {
        ApprovalStatus = 'A';
        ModifiedBy = approverId;
        ModifiedOn = DateTime.UtcNow;
        _approvals.Add(new RiskApproval
        {
            RiskId = Id,
            ApproverEmployeeSysId = approverId,
            Status = 'A',
            Remarks = remarks,
            LastModifiedBy = approverId,
            LastModifiedOn = DateTime.UtcNow,
            ApprovalType = 'R'
        });
        AddDomainEvent(new RiskApprovedEvent(Id, approverId));
    }

    public void Cancel(long cancelledBy, string reason)
    {
        CancelDate = DateTime.UtcNow;
        CancelReason = reason;
        ModifiedBy = cancelledBy;
        ModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new RiskCancelledEvent(Id, reason));
    }

    public void AddCause(RiskCause cause) => _causes.Add(cause);
    public void AddControl(RiskControl control) => _controls.Add(control);
    public void AddImpactMap(RiskImpactMap impactMap) => _impactMaps.Add(impactMap);
    public void AddEvent(RiskEvent riskEvent) => _events.Add(riskEvent);
    public void AddMonitor(RiskMonitor monitor) => _monitors.Add(monitor);
    public void AddMitigation(RiskMitigation mitigation)
    {
        _mitigations.Add(mitigation);
        MitigationFlag = 'Y';
        AddDomainEvent(new RiskMitigationAddedEvent(Id, mitigation.Id));
    }
}
