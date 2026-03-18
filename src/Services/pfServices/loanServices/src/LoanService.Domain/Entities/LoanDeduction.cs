using LoanService.Domain.Common;

namespace LoanService.Domain.Entities;

public class LoanDeduction : BaseEntity
{
    public long DedId { get; private set; }
    public long LoanNo { get; private set; }
    public decimal? ContributionId { get; private set; }
    public decimal DedAmount { get; private set; }
    public DateTime DedDate { get; private set; }

    // Navigation
    public LoanMain Loan { get; private set; } = null!;

    private LoanDeduction() { } // EF

    public LoanDeduction(long loanNo, decimal amount, DateTime date, decimal? contributionId = null)
    {
        LoanNo = loanNo;
        DedAmount = amount;
        DedDate = date;
        ContributionId = contributionId;
    }
}
