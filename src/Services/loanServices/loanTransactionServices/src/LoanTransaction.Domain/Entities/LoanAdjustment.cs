namespace LoanTransaction.Domain.Entities;

/// <summary>Maps to LOAN_ADJUSTMENT – disbursement adjustment against another loan</summary>
public class LoanAdjustment
{
    public long Id { get; set; }           // LOAN_ADJID
    public long LoanNo { get; set; }       // LOAN_NO
    public long AdjLoanNo { get; set; }    // LOAN_ADJLOANNO
    public decimal AdjPrincipalAmount { get; set; } // LOAN_ADJPRNAMT
    public decimal AdjInterestAmount { get; set; }  // LOAN_ADJINTAMT
    public long UpdatedBy { get; set; }    // LOAN_UPDATEDBY
    public DateTime UpdatedOn { get; set; } // LOAN_UPDATEDON
}
