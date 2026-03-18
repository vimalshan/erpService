using AuditService.Domain.Common;

namespace AuditService.Domain.Events;

public sealed class AuditCreatedEvent : DomainEvent
{
    public AuditCreatedEvent(long auditId, string auditName)
    {
        AuditId = auditId;
        AuditName = auditName;
    }

    public long AuditId { get; }
    public string AuditName { get; }
}

public sealed class AuditStatusChangedEvent : DomainEvent
{
    public AuditStatusChangedEvent(long auditId, char newStatus)
    {
        AuditId = auditId;
        NewStatus = newStatus;
    }

    public long AuditId { get; }
    public char NewStatus { get; }
}
