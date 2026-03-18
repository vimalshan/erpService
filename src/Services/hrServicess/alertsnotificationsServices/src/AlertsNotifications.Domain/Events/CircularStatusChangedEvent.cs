using AlertsNotifications.Domain.Common;

namespace AlertsNotifications.Domain.Events;

public sealed class CircularStatusChangedEvent : IDomainEvent
{
    public long CircularId { get; }
    public char OldStatus { get; }
    public char NewStatus { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public CircularStatusChangedEvent(long circularId, char oldStatus, char newStatus)
    {
        CircularId = circularId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}
