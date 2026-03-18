using AlertsNotifications.Domain.Common;

namespace AlertsNotifications.Domain.Events;

public sealed class AlertGroupCreatedEvent : IDomainEvent
{
    public decimal AlertGroupId { get; }
    public string AlertGroupName { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public AlertGroupCreatedEvent(decimal alertGroupId, string alertGroupName)
    {
        AlertGroupId = alertGroupId;
        AlertGroupName = alertGroupName;
    }
}
