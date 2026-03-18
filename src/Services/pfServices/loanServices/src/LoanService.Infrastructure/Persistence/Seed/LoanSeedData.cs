using LoanService.Domain.Entities;

namespace LoanService.Infrastructure.Persistence.Seed;

public static class LoanSeedData
{
    public static LoanMain[] GetSeedLoans()
    {
        return
        [
            CreateLoan(1001, "T01", 100001, 50000m, 1, "Housing repair", "12M", 50000m, 8.5m),
            CreateLoan(1002, "T01", 100002, 25000m, 2, "Medical emergency", "6M", 25000m, 7.0m),
            CreateLoan(1003, "T02", 100003, 100000m, 1, "Education", "24M", 100000m, 9.0m)
        ];
    }

    private static LoanMain CreateLoan(long loanNo, string trustCode, long memberId, decimal amount,
        long loanType, string reason, string tenure, decimal principalOs, decimal rate)
    {
        var loan = LoanMain.Create(loanNo, memberId, amount, loanType, reason, 1);
        loan.SetTrustCode(trustCode);
        loan.SetTenure(tenure);
        loan.SetRate(rate);
        loan.Approve(DateTime.UtcNow);
        loan.ClearDomainEvents();
        return loan;
    }
}
