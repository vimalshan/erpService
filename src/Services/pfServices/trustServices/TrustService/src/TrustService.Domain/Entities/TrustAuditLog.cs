using TrustService.Domain.Common;

namespace TrustService.Domain.Entities;

public class TrustAuditLog : BaseEntity
{
    public long AuditId { get; private set; }
    public string TrustCode { get; private set; } = string.Empty;
    public string AuditAction { get; private set; } = string.Empty;
    public string AuditTable { get; private set; } = string.Empty;
    public DateTime AuditTimestamp { get; private set; }
    public long AuditUserId { get; private set; }
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }

    public TrustMaster Trust { get; private set; } = null!;

    private TrustAuditLog() { }

    public static TrustAuditLog Create(string trustCode, string action, string table,
        long userId, string? oldValues = null, string? newValues = null)
    {
        return new TrustAuditLog
        {
            TrustCode = trustCode,
            AuditAction = action,
            AuditTable = table,
            AuditTimestamp = DateTime.UtcNow,
            AuditUserId = userId,
            OldValues = oldValues,
            NewValues = newValues
        };
    }
}
