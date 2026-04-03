namespace LoanTransaction.Domain.Entities;

/// <summary>Maps to LOAN_EMPINTRATEMAST – employee-specific interest rate for a loan</summary>
public class LoanEmpInterestRate
{
    public long Id { get; set; }              // LOANINT_RATEID
    public long LoanNo { get; set; }          // LOANINT_LOANNO
    public DateTime EffectiveDate { get; set; }   // LOANINT_EFFDATE
    public DateTime? ClosureDate { get; set; }    // LOANINT_CLSDATE
    public int Rate { get; set; }                 // LOANINT_RATE (%)
    public decimal EmiAmount { get; set; }        // LOANINT_EMIAMT
    public int NumberOfInstallments { get; set; } // LOANINT_INSNOS
    public long LastModifiedBy { get; set; }      // LOANINT_LASTMODIFIEDBY
    public DateTime LastModifiedOn { get; set; }  // LOANINT_LASTMODIFIEDON

    public bool IsActive => !ClosureDate.HasValue || ClosureDate.Value > DateTime.UtcNow;

    public void Close(long modifiedBy)
    {
        ClosureDate = DateTime.UtcNow;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
