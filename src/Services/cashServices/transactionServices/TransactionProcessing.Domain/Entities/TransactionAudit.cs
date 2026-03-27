using TransactionProcessing.Domain.Common;

namespace TransactionProcessing.Domain.Entities;

public class TransactionAudit : BaseEntity
{
    public long AuditId { get; private set; }
    public long TxnId { get; private set; }
    public string PreviousStatus { get; private set; } = string.Empty;
    public string NewStatus { get; private set; } = string.Empty;
    public string AuditAction { get; private set; } = string.Empty;
    public string? AuditRemarks { get; private set; }
    public long AuditBy { get; private set; }
    public DateTime AuditOn { get; private set; }

    public FinancialTransaction? Transaction { get; private set; }

    private TransactionAudit() { }

    public static TransactionAudit Create(long txnId, string previousStatus, string newStatus, string remarks, long auditBy)
    {
        return new TransactionAudit
        {
            TxnId = txnId,
            PreviousStatus = previousStatus,
            NewStatus = newStatus,
            AuditAction = $"{previousStatus} -> {newStatus}",
            AuditRemarks = remarks,
            AuditBy = auditBy,
            AuditOn = DateTime.UtcNow
        };
    }
}
