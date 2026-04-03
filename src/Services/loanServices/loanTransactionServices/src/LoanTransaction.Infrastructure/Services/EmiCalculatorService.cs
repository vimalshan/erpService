using LoanTransaction.Domain.Interfaces;

namespace LoanTransaction.Infrastructure.Services;

public class EmiCalculatorService : IEmiCalculatorService
{
    public decimal CalculateEmi(decimal principal, int ratePerAnnum, int tenureMonths)
    {
        if (tenureMonths <= 0) throw new ArgumentOutOfRangeException(nameof(tenureMonths));
        if (principal <= 0) throw new ArgumentOutOfRangeException(nameof(principal));

        if (ratePerAnnum == 0)
            return Math.Round(principal / tenureMonths, 2);

        decimal r = (decimal)ratePerAnnum / 12m / 100m;
        double rateDouble = (double)r;
        double factor = Math.Pow(1 + rateDouble, tenureMonths);
        decimal emi = principal * (decimal)(rateDouble * factor / (factor - 1));
        return Math.Round(emi, 2);
    }

    public IEnumerable<EmiScheduleItem> GenerateSchedule(
        decimal principal,
        int ratePerAnnum,
        int tenureMonths,
        DateTime firstInstallmentDate)
    {
        decimal emi = CalculateEmi(principal, ratePerAnnum, tenureMonths);
        decimal balance = principal;
        decimal r = (decimal)ratePerAnnum / 12m / 100m;
        var schedule = new List<EmiScheduleItem>(tenureMonths);

        for (int i = 1; i <= tenureMonths; i++)
        {
            decimal interest = Math.Round(balance * r, 2);
            decimal principalPart = i < tenureMonths
                ? Math.Round(emi - interest, 2)
                : balance; // last instalment clears residual

            balance -= principalPart;
            if (balance < 0) balance = 0;

            schedule.Add(new EmiScheduleItem
            {
                InstallmentNo = i,
                InstallmentDate = firstInstallmentDate.AddMonths(i - 1),
                InstallmentAmount = emi,
                PrincipalComponent = principalPart,
                InterestComponent = interest,
                PrincipalOutstanding = balance
            });
        }
        return schedule;
    }
}
