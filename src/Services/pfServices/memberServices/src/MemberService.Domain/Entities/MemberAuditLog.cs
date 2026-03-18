using MemberService.Domain.Common;

namespace MemberService.Domain.Entities;

public class MemberAuditLog
{
    public long AuditId { get; private set; }
    public long MemberNo { get; private set; }
    public string AuditAction { get; private set; } = string.Empty;
    public DateTime AuditTimestamp { get; private set; }
    public long AuditUserId { get; private set; }
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }

    private MemberAuditLog() { }

    public static MemberAuditLog Create(long memberNo, string action, long userId,
        string? oldValues = null, string? newValues = null) =>
        new()
        {
            MemberNo = memberNo,
            AuditAction = action,
            AuditTimestamp = DateTime.UtcNow,
            AuditUserId = userId,
            OldValues = oldValues,
            NewValues = newValues
        };
}
