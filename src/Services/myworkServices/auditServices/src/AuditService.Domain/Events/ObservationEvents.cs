using AuditService.Domain.Common;

namespace AuditService.Domain.Events;

public sealed class ObservationCreatedEvent : DomainEvent
{
    public ObservationCreatedEvent(long observationId, long auditId, string title)
    {
        ObservationId = observationId;
        AuditId = auditId;
        Title = title;
    }

    public long ObservationId { get; }
    public long AuditId { get; }
    public string Title { get; }
}

public sealed class ObservationStatusChangedEvent : DomainEvent
{
    public ObservationStatusChangedEvent(long observationId, long auditId, char oldStatus, char newStatus)
    {
        ObservationId = observationId;
        AuditId = auditId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }

    public long ObservationId { get; }
    public long AuditId { get; }
    public char OldStatus { get; }
    public char NewStatus { get; }
}
