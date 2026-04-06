using Microsoft.EntityFrameworkCore;
using PayTransactionalService.Domain.Entities;
using PayTransactionalService.Infrastructure.Persistence;

namespace PayTransactionalService.Infrastructure.SeedData;

public static class PayTransactionalDbContextSeed
{
    public static async Task SeedAsync(PayTransactionalDbContext context)
    {
        try
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.PayTransactions.AnyAsync())
            {
                Console.WriteLine("Database already seeded. Skipping...");
                return;
            }

            Console.WriteLine("Seeding database with initial data...");

            var currentMonth = $"{DateTime.UtcNow.Year:D4}-{DateTime.UtcNow.Month:D2}";
            var previousMonth = DateTime.UtcNow.AddMonths(-1);
            var prevMonthStr = $"{previousMonth.Year:D4}-{previousMonth.Month:D2}";

            // Seed arrears (allowances & deductions)
            var arrears = new List<PayArrear>
            {
                PayArrear.Create(1001, 25000m, "A", currentMonth, "admin", "HRA", "House Rent Allowance"),
                PayArrear.Create(1001, 5000m, "A", currentMonth, "admin", "DA", "Dearness Allowance"),
                PayArrear.Create(1001, 1800m, "D", currentMonth, "admin", "PF", "Provident Fund"),
                PayArrear.Create(1001, 750m, "D", currentMonth, "admin", "ESI", "Employee State Insurance"),
                PayArrear.Create(1002, 30000m, "A", currentMonth, "admin", "HRA", "House Rent Allowance"),
                PayArrear.Create(1002, 7000m, "A", currentMonth, "admin", "DA", "Dearness Allowance"),
                PayArrear.Create(1002, 2160m, "D", currentMonth, "admin", "PF", "Provident Fund"),
                PayArrear.Create(1003, 20000m, "A", currentMonth, "admin", "HRA", "House Rent Allowance"),
                PayArrear.Create(1003, 3500m, "A", currentMonth, "admin", "DA", "Dearness Allowance"),
                PayArrear.Create(1003, 1500m, "D", currentMonth, "admin", "PF", "Provident Fund"),
            };
            context.PayArrears.AddRange(arrears);
            await context.SaveChangesAsync();
            Console.WriteLine($"Seeded {arrears.Count} arrear records.");

            // Seed pay transactions
            var transactions = new List<PayTransaction>
            {
                PayTransaction.Create(1001, prevMonthStr, 75000m, 12500m, "admin"),
                PayTransaction.Create(1002, prevMonthStr, 90000m, 15000m, "admin"),
                PayTransaction.Create(1003, prevMonthStr, 60000m, 10000m, "admin"),
                PayTransaction.Create(1001, currentMonth, 75000m, 12550m, "admin"),
                PayTransaction.Create(1002, currentMonth, 90000m, 15160m, "admin"),
                PayTransaction.Create(1003, currentMonth, 60000m, 10500m, "admin"),
            };
            context.PayTransactions.AddRange(transactions);
            await context.SaveChangesAsync();
            Console.WriteLine($"Seeded {transactions.Count} transaction records.");

            // Complete previous month transactions
            var prevTxns = transactions.Where(t => t.MonthYear == prevMonthStr).ToList();
            foreach (var txn in prevTxns) txn.Complete();
            await context.SaveChangesAsync();

            // Seed a completed batch for prev month
            var batch = PayrollBatch.Create(prevMonthStr, "admin");
            context.PayrollBatches.Add(batch);
            await context.SaveChangesAsync();
            batch.Complete(prevTxns.Count);
            await context.SaveChangesAsync();
            Console.WriteLine("Seeded 1 payroll batch (previous month, completed).");

            // Seed adjustments
            var adjustments = new List<PayAdjustment>
            {
                PayAdjustment.Create(1001, "BONUS", 15000m, currentMonth, DateTime.UtcNow, "admin", "Quarterly bonus"),
                PayAdjustment.Create(1002, "INCREMENT", 5000m, currentMonth, DateTime.UtcNow, "admin", "Annual increment"),
                PayAdjustment.Create(1003, "CORRECTION", -2000m, currentMonth, DateTime.UtcNow, "admin", "Overpayment correction"),
            };
            context.PayAdjustments.AddRange(adjustments);
            await context.SaveChangesAsync();
            Console.WriteLine($"Seeded {adjustments.Count} adjustment records.");

            // Approve the bonus
            adjustments[0].Approve(9999);
            await context.SaveChangesAsync();

            Console.WriteLine("Database seeding completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding database: {ex.Message}");
            throw;
        }
    }
}
