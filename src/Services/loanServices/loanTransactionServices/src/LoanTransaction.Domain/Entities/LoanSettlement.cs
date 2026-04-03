namespace LoanTransaction.Domain.Entities;

/// <summary>Maps to LOAN_SET table – settlement/payment record per instalment</summary>
public class LoanSettlement
{
    public long Id { get; set; }              // LOANSET_ID
    public long UnitId { get; set; }          // LOANSET_UNITID
    public long LoanNo { get; set; }          // LOANSET_LOANNO
    public string SettlementType { get; set; } = "INS"; // LOANSET_TYPE: SET/INS
    public long InstallmentNo { get; set; }   // LOANSET_INSNO
    public DateTime InstallmentDate { get; set; } // LOANSET_INSDATE
    public DateTime RecoveryDate { get; set; }    // LOANSET_RECDATE
    public string RecoveryType { get; set; } = "PRN"; // LOANSET_RECTYPE: PRN/INT
    public decimal InstallmentAmount { get; set; }     // LOANSET_INSAMT
    public string PayType { get; set; } = "PAY";       // LOANSET_PAYTYPE: DIR/PAY/ADJ
    public long PayBatchId { get; set; }               // LOANSET_PAYBATCHID
    public int PayId { get; set; }                     // LOANSET_PAYID
    public long AdjustLoanNo { get; set; }             // LOANSET_ADJLOANNO
    public DateTime? CancelDate { get; set; }          // LOANSET_CANCELDATE
    public long? CancelBy { get; set; }                // LOANSET_CANCELBY
    public long UpdatedBy { get; set; }                // LOANSET_UPDATEDBY
    public DateTime UpdatedOn { get; set; }            // LOANSET_UPDATEDON

    public bool IsCancelled => CancelDate.HasValue;

    public void Cancel(long cancelledBy)
    {
        if (IsCancelled) throw new InvalidOperationException("Settlement is already cancelled.");
        CancelDate = DateTime.UtcNow;
        CancelBy = cancelledBy;
    }
}
