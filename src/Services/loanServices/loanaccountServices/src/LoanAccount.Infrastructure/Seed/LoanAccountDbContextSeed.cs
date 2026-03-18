using LoanAccount.Domain.Entities;
using LoanAccount.Domain.ValueObjects;
using LoanAccount.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanAccount.Infrastructure.Seed;

/// <summary>
/// Seed data for initial database population
/// </summary>
public static class LoanAccountDbContextSeed
{
    public static async Task SeedAsync(LoanAccountDbContext context)
    {
        if (await context.LoanMains.AnyAsync())
            return; // Database already seeded

        // Seed LoanMain entities only for now
        var loans = new List<LoanMain>
        {
            LoanMain.Create(
                loanNo: 1001,
                loanAppId: 100,
                empSysId: 1,
                loanId: 1,
                gradeId: 5,
                principalAmount: 100000,
                disbursementType: DisbursementType.New,
                loanDate: DateTime.UtcNow.AddMonths(-6),
                firstInstallmentDate: DateTime.UtcNow.AddMonths(-5),
                unitId: 1,
                subClassId: 1,
                reason: "Educational expenses",
                guarantorId: 2,
                createdBy: 1),

            LoanMain.Create(
                loanNo: 1002,
                loanAppId: 101,
                empSysId: 2,
                loanId: 2,
                gradeId: 4,
                principalAmount: 50000,
                disbursementType: DisbursementType.New,
                loanDate: DateTime.UtcNow.AddMonths(-3),
                firstInstallmentDate: DateTime.UtcNow.AddMonths(-2),
                unitId: 1,
                subClassId: 1,
                reason: "Medical expenses",
                guarantorId: 1,
                createdBy: 1),

            LoanMain.Create(
                loanNo: 1003,
                loanAppId: 102,
                empSysId: 3,
                loanId: 3,
                gradeId: 6,
                principalAmount: 200000,
                disbursementType: DisbursementType.New,
                loanDate: DateTime.UtcNow.AddMonths(-1),
                firstInstallmentDate: DateTime.UtcNow,
                unitId: 2,
                subClassId: 2,
                reason: "Home purchase",
                guarantorId: 4,
                createdBy: 1)
        };

        foreach (var loan in loans)
        {
            await context.LoanMains.AddAsync(loan);
        }

        await context.SaveChangesAsync();
    }
}
