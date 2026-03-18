namespace ReportingService.Domain.Events;

/// <summary>
/// Event fired when an appraisal is created
/// </summary>
public class AppraisalCreatedEvent : DomainEvent
{
    public string? UserName { get; }
    public string? UserId { get; }

    public AppraisalCreatedEvent(long aggregateId, string? userName, string? userId)
        : base(aggregateId, DateTime.UtcNow)
    {
        UserName = userName;
        UserId = userId;
    }
}
