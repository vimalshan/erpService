using DispatchPlanning.Domain.Entities;
using DispatchPlanning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DispatchPlanning.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(DispatchPlanningDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.DispatchPlanMainGroups.AnyAsync()) return;

        // Seed Main Groups
        var mainGroups = new[]
        {
            DispatchPlanMainGroup.Create(0, "PASSENGER CAR", 'P', 'Y', "TOTAL PC", 1, 1, 1),
            DispatchPlanMainGroup.Create(0, "UTILITY VEHICLE", 'U', 'Y', "TOTAL UV", 2, 1, 1),
            DispatchPlanMainGroup.Create(0, "COMMERCIAL VEH", 'C', 'N', "TOTAL CV", 3, 1, 1)
        };
        context.DispatchPlanMainGroups.AddRange(mainGroups);
        await context.SaveChangesAsync();

        // Seed Sub Groups (use generated main group IDs)
        var subGroups = new[]
        {
            DispatchPlanSubGroup.Create(0, mainGroups[0].MainGroupId, "SEDAN",     101, 1, 'N', 1),
            DispatchPlanSubGroup.Create(0, mainGroups[0].MainGroupId, "HATCHBACK", 102, 2, 'N', 1),
            DispatchPlanSubGroup.Create(0, mainGroups[1].MainGroupId, "SUV",       201, 1, 'N', 1),
            DispatchPlanSubGroup.Create(0, mainGroups[1].MainGroupId, "MUV",       202, 2, 'N', 1),
            DispatchPlanSubGroup.Create(0, mainGroups[2].MainGroupId, "TRUCK",     301, 1, 'N', 1)
        };
        context.DispatchPlanSubGroups.AddRange(subGroups);
        await context.SaveChangesAsync();

        // Seed Breakup Items (use generated sub group IDs)
        var breakupItems = new[]
        {
            DispatchPlanBreakupItem.Create(0, subGroups[0].SubGroupId, 101, "SEDAN - STANDARD",     1, 1, 1, DateTime.UtcNow.AddDays(-90), null, 1),
            DispatchPlanBreakupItem.Create(0, subGroups[1].SubGroupId, 102, "HATCHBACK - STANDARD", 1, 1, 2, DateTime.UtcNow.AddDays(-90), null, 1),
            DispatchPlanBreakupItem.Create(0, subGroups[2].SubGroupId, 201, "SUV - STANDARD",       1, 1, 1, DateTime.UtcNow.AddDays(-90), null, 1)
        };
        context.DispatchPlanBreakupItems.AddRange(breakupItems);
        await context.SaveChangesAsync();
    }
}
