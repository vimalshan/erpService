using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Infrastructure.Data;

namespace TransactionService.Infrastructure;

public static class SeedDataExtensions
{
    public static async Task SeedTransactionDataAsync(this TransactionDbContext context)
    {
        // Seed SaaLevels
        if (!context.SaaLevels.Any())
        {
            var levels = new List<SaaLevel>
            {
                new SaaLevel("Level A", "5000", "Outstanding performance", 0, 5000, DateTime.UtcNow.AddYears(-1), 1001),
                new SaaLevel("Level B", "3000", "Above average performance", 0, 3000, DateTime.UtcNow.AddYears(-1), 1001),
                new SaaLevel("Level C", "1500", "Average performance", 0, 1500, DateTime.UtcNow.AddYears(-1), 1001)
            };

            await context.SaaLevels.AddRangeAsync(levels);
            await context.SaveChangesAsync();
        }

        // Seed SaaPeriods
        if (!context.SaaPeriods.Any())
        {
            var periods = new List<SaaPeriod>
            {
                new SaaPeriod(2025, 1, DateTime.UtcNow.AddMonths(-3), DateTime.UtcNow.AddMonths(3), DateTime.UtcNow.AddMonths(-2)),
                new SaaPeriod(2025, 2, DateTime.UtcNow.AddMonths(3), DateTime.UtcNow.AddMonths(6), DateTime.UtcNow.AddMonths(4))
            };

            await context.SaaPeriods.AddRangeAsync(periods);
            await context.SaveChangesAsync();
        }

        // Seed SaaBudgets
        if (!context.SaaBudgets.Any())
        {
            var budgets = new List<SaaBudget>
            {
                new SaaBudget(1, 2025, 500000m, 1001),
                new SaaBudget(2, 2025, 300000m, 1001)
            };

            await context.SaaBudgets.AddRangeAsync(budgets);
            await context.SaveChangesAsync();
        }

        // Seed DemandMasters
        if (!context.DemandMasters.Any())
        {
            var demands = new List<DemandMaster>
            {
                new DemandMaster("Hiring", 101, "Need 5 developers for project X", DateTime.UtcNow.AddMonths(1), "High", 1001),
                new DemandMaster("Training", 102, "Technical training for Q2", DateTime.UtcNow.AddMonths(2), "Medium", 1002),
                new DemandMaster("Equipment", 101, "Laptop procurement for new hires", DateTime.UtcNow.AddDays(15), "Low", 1001)
            };

            await context.DemandMasters.AddRangeAsync(demands);
            await context.SaveChangesAsync();
        }
    }
}
