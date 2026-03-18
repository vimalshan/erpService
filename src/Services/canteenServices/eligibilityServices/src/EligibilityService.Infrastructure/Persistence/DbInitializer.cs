using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EligibilityService.Domain.Entities;
using EligibilityService.Infrastructure.Persistence;

namespace EligibilityService.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(EligibilityDbContext context, ILogger logger)
    {
        await context.Database.MigrateAsync();

        // ── Seed ShiftMappings ────────────────────────────────────────────────
        if (!await context.ShiftMappings.AnyAsync())
        {
            var shiftMappings = new[]
            {
                ShiftMapping.Create(10001, "A", "N", "B"),
                ShiftMapping.Create(10001, "B", "A", "C"),
                ShiftMapping.Create(10001, "C", "B", "G"),
                ShiftMapping.Create(10001, "G", "C", "A")
            };

            await context.ShiftMappings.AddRangeAsync(shiftMappings);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded ShiftMappings.");
        }

        // ── Seed EligibilityMasters ────────────────────────────────────────────
        if (!await context.EligibilityMasters.AnyAsync())
        {
            var masters = new[]
            {
                EligibilityMaster.Create(10001, "A", 1001, 2, 99001, "T01"),
                EligibilityMaster.Create(10001, "B", 1001, 2, 99001, "T01"),
                EligibilityMaster.Create(10001, "C", 1002, 1, 99001, "T01"),
                EligibilityMaster.Create(10001, "G", 1002, 1, 99001, "T01"),
            };

            foreach (var m in masters)
                m.ClearDomainEvents();

            await context.EligibilityMasters.AddRangeAsync(masters);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded EligibilityMasters.");
        }
    }
}
