using MediatR;

namespace AuditService.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

public class AuditCreatedEvent : IDomainEvent, INotification
{
    public int AuditId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public AuditCreatedEvent(int auditId) => AuditId = auditId;
}

public class AuditStatusChangedEvent : IDomainEvent, INotification
{
    public int AuditId { get; }
    public string OldStatus { get; }
    public string NewStatus { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public AuditStatusChangedEvent(int auditId, string oldStatus, string newStatus)
    {
        AuditId = auditId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}

public class AuditCompletedEvent : IDomainEvent, INotification
{
    public int AuditId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public AuditCompletedEvent(int auditId) => AuditId = auditId;
}
