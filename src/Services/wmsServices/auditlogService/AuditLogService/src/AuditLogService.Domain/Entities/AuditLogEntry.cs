using AuditLogService.Domain.Common;
using AuditLogService.Domain.Events;
using AuditLogService.Domain.ValueObjects;

namespace AuditLogService.Domain.Entities;

public class AuditLogEntry : AggregateRoot<long>
{
    public string TableName { get; private set; } = null!;
    public int RecordId { get; private set; }
    public AuditAction Action { get; private set; } = null!;
    public string? ChangedBy { get; private set; }
    public DateTime ChangeDate { get; private set; }
    public ChangeData ChangeData { get; private set; } = null!;

    private AuditLogEntry() { } // EF constructor

    public static AuditLogEntry Create(
        string tableName,
        int recordId,
        string action,
        string? changedBy,
        string? oldValues,
        string? newValues)
    {
        var entry = new AuditLogEntry
        {
            TableName = tableName,
            RecordId = recordId,
            Action = AuditAction.From(action),
            ChangedBy = changedBy,
            ChangeDate = DateTime.UtcNow,
            ChangeData = new ChangeData(oldValues, newValues)
        };

        entry.AddDomainEvent(new AuditLogCreatedEvent(entry));
        return entry;
    }
}
