using RiskService.Domain.Common;

namespace RiskService.Domain.Events;

public class RiskCreatedEvent(long riskId, string title) : DomainEvent
{
    public long RiskId { get; } = riskId;
    public string Title { get; } = title;
}

public class RiskSubmittedEvent(long riskId, long submittedBy) : DomainEvent
{
    public long RiskId { get; } = riskId;
    public long SubmittedBy { get; } = submittedBy;
}

public class RiskApprovedEvent(long riskId, long approvedBy) : DomainEvent
{
    public long RiskId { get; } = riskId;
    public long ApprovedBy { get; } = approvedBy;
}

public class RiskCancelledEvent(long riskId, string reason) : DomainEvent
{
    public long RiskId { get; } = riskId;
    public string Reason { get; } = reason;
}

public class RiskMitigationAddedEvent(long riskId, long mitigationId) : DomainEvent
{
    public long RiskId { get; } = riskId;
    public long MitigationId { get; } = mitigationId;
}

public class SelfAssessmentCompletedEvent(long assessmentId) : DomainEvent
{
    public long AssessmentId { get; } = assessmentId;
}
