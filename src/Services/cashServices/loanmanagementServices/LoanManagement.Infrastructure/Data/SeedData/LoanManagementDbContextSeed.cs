using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LoanManagement.Infrastructure.Data.SeedData;

public static class LoanManagementDbContextSeed
{
    public static async Task SeedAsync(LoanManagementDbContext context, ILogger logger)
    {
        if (await context.LoanMain.AnyAsync())
        {
            logger.LogInformation("Seed data already present — skipping.");
            return;
        }

        logger.LogInformation("Seeding LoanManagement data...");

        // ── Loan 1 ────────────────────────────────────────────────────────────
        var loan1 = LoanMain.Create(
            loanId: 1,
            loanKey: "L2026-001",
            orgId: 100,
            loanAmount: 5_000_000m,
            loanTypeId: 10,
            bankId: 200,
            createdBy: 1,
            loanDate: new DateTime(2026, 1, 15),
            orgCurr: 1m,
            loanCurr: 1m);

        // 3 disbursement tranches — DISB_ID is IDENTITY, pass 0 so EF lets SQL Server generate it
        var d1 = LoanDisbursementSchedule.Create(0, 1, new DateTime(2026, 2, 1),  2_000_000m, 1m);
        var d2 = LoanDisbursementSchedule.Create(0, 1, new DateTime(2026, 5, 1),  2_000_000m, 1m);
        var d3 = LoanDisbursementSchedule.Create(0, 1, new DateTime(2026, 9, 1),  1_000_000m, 1m);
        loan1.AddDisbursement(d1);
        loan1.AddDisbursement(d2);
        loan1.AddDisbursement(d3);

        // 2 interest records — INT_ID is IDENTITY, pass 0
        var i1 = LoanInterest.Create(0, 1, InterestRateType.Fixed,    8.5m,  null, new DateTime(2026, 1, 15));
        var i2 = LoanInterest.Create(0, 1, InterestRateType.Floating, 1.0m,  301,  new DateTime(2026, 7, 1));
        loan1.AddInterest(i1);
        loan1.AddInterest(i2);

        // 36-month EMI repayment schedule — REPAY_ID is IDENTITY, pass 0
        decimal emi = 152_777m;
        for (int m = 0; m < 36; m++)
        {
            var repay = LoanRepaymentSchedule.Create(
                repayId: 0,
                loanId: 1,
                repayDate: new DateTime(2026, 2, 1).AddMonths(m),
                amount: emi);
            loan1.AddRepayment(repay);
        }

        // ── Loan 2 ────────────────────────────────────────────────────────────
        var loan2 = LoanMain.Create(
            loanId: 2,
            loanKey: "L2026-002",
            orgId: 101,
            loanAmount: 1_200_000m,
            loanTypeId: 11,
            bankId: 201,
            createdBy: 1,
            loanDate: new DateTime(2026, 3, 1),
            orgCurr: 1m,
            loanCurr: 1m);

        var d4 = LoanDisbursementSchedule.Create(0, 2, new DateTime(2026, 3, 15), 1_200_000m, 1m);
        loan2.AddDisbursement(d4);

        var i3 = LoanInterest.Create(0, 2, InterestRateType.Fixed, 9.25m, null, new DateTime(2026, 3, 1));
        loan2.AddInterest(i3);

        for (int m = 0; m < 12; m++)
        {
            var repay = LoanRepaymentSchedule.Create(
                repayId: 0,
                loanId: 2,
                repayDate: new DateTime(2026, 4, 1).AddMonths(m),
                amount: 104_167m);
            loan2.AddRepayment(repay);
        }

        // Clear all domain events before persisting — seed should not publish events
        loan1.ClearDomainEvents();
        loan2.ClearDomainEvents();

        context.LoanMain.AddRange(loan1, loan2);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeding completed: 2 loans, 4 disbursements, 3 interest records, 48 repayment lines.");
    }
}
