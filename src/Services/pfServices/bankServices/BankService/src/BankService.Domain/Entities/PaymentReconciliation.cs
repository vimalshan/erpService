using BankService.Domain.Common;

namespace BankService.Domain.Entities;

public class PaymentReconciliation : BaseEntity
{
    public long ReconId { get; private set; }
    public long ChequeId { get; private set; }
    public string ReconReference { get; private set; } = null!;
    public decimal ReconAmount { get; private set; }
    public DateTime ReconDate { get; private set; }
    public string ReconStatus { get; private set; } = "O";

    // Navigation
    public ChequeDetail Cheque { get; private set; } = null!;

    private PaymentReconciliation() { }

    public static PaymentReconciliation Create(long chequeId, string reconReference,
        decimal reconAmount, DateTime reconDate)
    {
        return new PaymentReconciliation
        {
            ChequeId = chequeId,
            ReconReference = reconReference,
            ReconAmount = reconAmount,
            ReconDate = reconDate,
            ReconStatus = "O"
        };
    }

    public void MarkReconciled() => ReconStatus = "R";
    public void MarkFailed() => ReconStatus = "F";
}
