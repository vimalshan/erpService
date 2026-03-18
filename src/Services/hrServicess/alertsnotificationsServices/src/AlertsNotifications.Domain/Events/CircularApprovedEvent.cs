using AlertsNotifications.Domain.Common;

namespace AlertsNotifications.Domain.Events;

public sealed class CircularApprovedEvent : IDomainEvent
{
    public long CircularId { get; }
    public string CircularSubject { get; }
    public long ApprovedBy { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public CircularApprovedEvent(long circularId, string circularSubject, long approvedBy)
    {
        CircularId = circularId;
        CircularSubject = circularSubject;
        ApprovedBy = approvedBy;
    }
}
