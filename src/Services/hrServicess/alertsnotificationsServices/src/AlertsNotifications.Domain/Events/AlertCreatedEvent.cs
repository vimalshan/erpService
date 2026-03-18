using AlertsNotifications.Domain.Common;

namespace AlertsNotifications.Domain.Events;

public sealed class AlertCreatedEvent : IDomainEvent
{
    public decimal AlertId { get; }
    public string AlertName { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public AlertCreatedEvent(decimal alertId, string alertName)
    {
        AlertId = alertId;
        AlertName = alertName;
    }
}
