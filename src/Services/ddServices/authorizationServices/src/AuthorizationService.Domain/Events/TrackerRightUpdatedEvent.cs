namespace AuthorizationService.Domain.Events;

/// <summary>
/// Event fired when tracker rights are updated
/// </summary>
public class TrackerRightUpdatedEvent : DomainEvent
{
    public string? UserId { get; }
    public string? BusinessCode { get; }

    public TrackerRightUpdatedEvent(long aggregateId, string? userId, string? businessCode)
        : base(aggregateId, DateTime.UtcNow)
    {
        UserId = userId;
        BusinessCode = businessCode;
    }
}
