using Microsoft.EntityFrameworkCore;
using SettlementService.Domain.Aggregates;
using SettlementService.Infrastructure.Persistence.EfCore;

namespace SettlementService.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedAsync(SettlementDbContext context)
    {
        if (await context.Settlements.AnyAsync())
            return;

        var settlements = new[]
        {
            new Settlement(1001, 500001, "F", 250000m, new DateTime(2026, 1, 15), 1, "PF1", "Retirement"),
            new Settlement(1002, 500002, "P", 120000m, new DateTime(2026, 2, 10), 1, "PF1", "Partial withdrawal"),
            new Settlement(1003, 500003, "F", 380000m, new DateTime(2026, 3, 1), 2, "PF2", "Superannuation"),
        };

        // Clear domain events from constructor
        foreach (var s in settlements)
            s.ClearDomainEvents();

        await context.Settlements.AddRangeAsync(settlements);
        await context.SaveChangesAsync();
    }
}
