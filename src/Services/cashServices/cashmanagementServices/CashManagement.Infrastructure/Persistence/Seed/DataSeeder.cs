using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CashManagement.Domain.Entities;
using CashManagement.Domain.ValueObjects;

namespace CashManagement.Infrastructure.Persistence.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(CashManagementDbContext context, ILogger logger)
    {
        try
        {
            await SeedCashUnitsAsync(context);
            await SeedBankAccountsAsync(context);
            await context.SaveChangesAsync();
            logger.LogInformation("Cash Management seed data applied successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding Cash Management data.");
            throw;
        }
    }

    private static async Task SeedCashUnitsAsync(CashManagementDbContext context)
    {
        if (await context.CashUnits.AnyAsync()) return;

        var cashUnits = new List<CashUnit>
        {
            CashUnit.Create(1, "Petty Cash - Office A", "PCF-OA", "Office A", 1001, 50_000, 1),
            CashUnit.Create(2, "Sales Till - Main Branch", "ST-MB", "Main Branch", 1002, 100_000, 1)
        };

        await context.CashUnits.AddRangeAsync(cashUnits);
    }

    private static async Task SeedBankAccountsAsync(CashManagementDbContext context)
    {
        if (await context.BankAccounts.AnyAsync()) return;

        var bankAccounts = new List<BankAccount>
        {
            BankAccount.Create(1, "First National Bank", "FNB-001-123456", "Main Branch", "Current", 1001),
            BankAccount.Create(2, "Commerce Bank", "CMB-002-654321", "City Centre Branch", "Savings", 1001)
        };

        await context.BankAccounts.AddRangeAsync(bankAccounts);
    }
}
