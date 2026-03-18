namespace ReportingService.Domain.Events;

/// <summary>
/// Event fired when an appraisal is completed
/// </summary>
public class AppraisalCompletedEvent : DomainEvent
{
    public DateTime CompletedAt { get; }

    public AppraisalCompletedEvent(long aggregateId, DateTime completedAt)
        : base(aggregateId, completedAt)
    {
        CompletedAt = completedAt;
        Version = 2;
    }
}
