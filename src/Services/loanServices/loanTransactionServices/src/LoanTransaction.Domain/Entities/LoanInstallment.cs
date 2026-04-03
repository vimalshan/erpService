namespace LoanTransaction.Domain.Entities;

/// <summary>Maps to LOAN_INS table – one row per EMI instalment</summary>
public class LoanInstallment
{
    public long Id { get; set; }                     // LOANINS_ID
    public long LoanNo { get; set; }                 // LOANINS_LOANNO
    public long UnitId { get; set; }                 // LOANINS_UNITID
    public DateTime InstallmentDate { get; set; }    // LOANINS_INSDATE
    public long InstallmentNo { get; set; }          // LOANINS_INSNO
    public decimal InstallmentAmount { get; set; }   // LOANINS_INSAMT
    public decimal PrincipalOutstanding { get; set; }// LOANINS_PRNOUT
    public decimal PrincipalAdjusted { get; set; }   // LOANINS_PRNADJ
    public decimal InterestAdjusted { get; set; }    // LOANINS_INTADJ
    public DateTime? FromDate { get; set; }          // LOANINS_FRODATE
    public decimal InterestAccrued { get; set; }     // LOANINS_INTACC
    public decimal InterestRecovered { get; set; }   // LOANINS_INTREC
    public decimal PrincipalRecovered { get; set; }  // LOANINS_PRNREC
    public int InterestRate { get; set; }            // LOANINS_INTRATE
    public string Remarks { get; set; } = string.Empty; // LOANINS_REMARKS
    public long UpdatedBy { get; set; }              // LOANINS_UPDATEDBY
    public DateTime UpdatedOn { get; set; }          // LOANINS_UPDATEDON

    public bool IsPaid => PrincipalRecovered > 0 || InterestRecovered > 0;
    public decimal RemainingAmount => InstallmentAmount - PrincipalRecovered - InterestRecovered;

    public void RecordPayment(decimal principalPaid, decimal interestPaid, long paidBy)
    {
        if (principalPaid < 0) throw new ArgumentException("Principal paid cannot be negative.");
        if (interestPaid < 0) throw new ArgumentException("Interest paid cannot be negative.");

        PrincipalRecovered += principalPaid;
        InterestRecovered += interestPaid;
        PrincipalAdjusted = principalPaid;
        InterestAdjusted = interestPaid;
        UpdatedBy = paidBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
