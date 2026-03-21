using AuditLogService.Domain.Common;
using AuditLogService.Domain.Entities;

namespace AuditLogService.Domain.Events;

public sealed class AuditLogCreatedEvent : IDomainEvent
{
    public AuditLogEntry AuditLog { get; }

    public AuditLogCreatedEvent(AuditLogEntry auditLog)
    {
        AuditLog = auditLog;
    }
}
