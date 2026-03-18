using BankService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankService.Infrastructure.Persistence;

public static class BankDbContextSeed
{
    public static async Task SeedAsync(BankDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.BankMasters.AnyAsync())
        {
            var banks = new[]
            {
                BankMaster.Create("001", "SBI001", "State Bank of India", "400002001",
                    "Main Branch", "Parliament Street, New Delhi", DateTime.Parse("2020-01-01")),
                BankMaster.Create("001", "PNB001", "Punjab National Bank", "110024002",
                    "Connaught Place", "Block A, Connaught Place, New Delhi", DateTime.Parse("2020-01-01")),
                BankMaster.Create("002", "BOB001", "Bank of Baroda", "390002003",
                    "Head Office", "Mandvi, Baroda", DateTime.Parse("2020-06-15")),
            };

            await context.BankMasters.AddRangeAsync(banks);
        }

        if (!await context.BankAccounts.AnyAsync())
        {
            var accounts = new[]
            {
                BankAccount.Create("ACC-SBI-001", "PF Trust SBI Account", "SBI001", "001", "Savings", DateTime.Parse("2020-01-15")),
                BankAccount.Create("ACC-PNB-001", "PF Trust PNB Account", "PNB001", "001", "Current", DateTime.Parse("2020-02-01")),
                BankAccount.Create("ACC-BOB-001", "PF Trust BOB Account", "BOB001", "002", "Savings", DateTime.Parse("2020-07-01")),
            };

            await context.BankAccounts.AddRangeAsync(accounts);
        }

        await context.SaveChangesAsync();
    }
}
