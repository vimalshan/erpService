using LoanService.Domain.Common;

namespace LoanService.Domain.Entities;

public class LoanRepayment : BaseEntity
{
    public long RepayId { get; private set; }
    public long LoanNo { get; private set; }
    public int InstallmentNo { get; private set; }
    public decimal RepayAmount { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? PaidDate { get; private set; }
    public decimal? PaidAmount { get; private set; }
    public char Status { get; private set; } = 'O';

    // Navigation
    public LoanMain Loan { get; private set; } = null!;

    private LoanRepayment() { } // EF

    public LoanRepayment(long loanNo, int installmentNo, decimal amount, DateTime dueDate)
    {
        LoanNo = loanNo;
        InstallmentNo = installmentNo;
        RepayAmount = amount;
        DueDate = dueDate;
        Status = 'O';
    }

    public void MarkPaid(decimal paidAmount, DateTime paidDate)
    {
        PaidAmount = paidAmount;
        PaidDate = paidDate;
        Status = 'P';
    }
}
