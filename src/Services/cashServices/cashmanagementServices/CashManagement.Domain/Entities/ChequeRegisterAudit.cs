using CashManagement.Domain.Common;

namespace CashManagement.Domain.Entities;

public class ChequeRegisterAudit : BaseEntity
{
    public long AuditId { get; private set; }
    public long ChequeId { get; private set; }
    public long BankAccountId { get; private set; }
    public string ChequeNumber { get; private set; } = default!;
    public string? PreviousStatus { get; private set; }
    public string NewStatus { get; private set; } = default!;
    public string AuditAction { get; private set; } = default!;
    public DateTime AuditDate { get; private set; }

    private ChequeRegisterAudit() { }

    public static ChequeRegisterAudit Create(long chequeId, long bankAccountId, string chequeNumber,
        string? previousStatus, string newStatus, string action)
        => new()
        {
            ChequeId = chequeId,
            BankAccountId = bankAccountId,
            ChequeNumber = chequeNumber,
            PreviousStatus = previousStatus,
            NewStatus = newStatus,
            AuditAction = action,
            AuditDate = DateTime.UtcNow
        };
}
