using GroupIncentiveService.Domain.Entities;
using GroupIncentiveService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GroupIncentiveService.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, ILogger logger)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GroupIncentiveDbContext>();

        await context.Database.MigrateAsync();

        if (await context.GroupMasters.AnyAsync()) return;

        logger.LogInformation("Seeding initial data...");

        // ── Seed Groups ──────────────────────────────────────────
        var groups = new[]
        {
            GroupMaster.Create(1, "Production Team Alpha", "Main production group A", new DateTime(2024, 1, 1), 1),
            GroupMaster.Create(2, "Production Team Beta",  "Main production group B", new DateTime(2024, 1, 1), 1),
            GroupMaster.Create(3, "QA Team",              "Quality assurance group",  new DateTime(2024, 1, 1), 1),
        };
        await context.GroupMasters.AddRangeAsync(groups);

        // ── Seed Employee Mappings ────────────────────────────────
        var mappings = new[]
        {
            GroupEmployeeMap.Create(1, 1, 1001, new DateTime(2024, 1, 1), "Leader",  1),
            GroupEmployeeMap.Create(2, 1, 1002, new DateTime(2024, 1, 1), "Member",  1),
            GroupEmployeeMap.Create(3, 1, 1003, new DateTime(2024, 1, 1), "Member",  1),
            GroupEmployeeMap.Create(4, 2, 1004, new DateTime(2024, 1, 1), "Leader",  1),
            GroupEmployeeMap.Create(5, 2, 1005, new DateTime(2024, 1, 1), "Member",  1),
            GroupEmployeeMap.Create(6, 3, 1006, new DateTime(2024, 1, 1), "Leader",  1),
        };
        await context.GroupEmployeeMaps.AddRangeAsync(mappings);

        // ── Seed Incentive Break Rules ────────────────────────────
        var breaks = new[]
        {
            GroupIncentiveBreak.Create(1, 1, 90m, 100m, new DateTime(2024, 1, 1), 1),
            GroupIncentiveBreak.Create(2, 1, 75m,  80m, new DateTime(2024, 1, 1), 1),
            GroupIncentiveBreak.Create(3, 1, 60m,  60m, new DateTime(2024, 1, 1), 1),
            GroupIncentiveBreak.Create(4, 2, 90m, 100m, new DateTime(2024, 1, 1), 1),
            GroupIncentiveBreak.Create(5, 2, 75m,  80m, new DateTime(2024, 1, 1), 1),
        };
        await context.GroupIncentiveBreaks.AddRangeAsync(breaks);

        // ── Seed a Sample Incentive Record ───────────────────────
        var incentiveMain = GroupIncentiveMain.Create(1, 1, 1, 2026, 50000m, 1);
        await context.GroupIncentiveMains.AddAsync(incentiveMain);

        var details = new[]
        {
            GroupIncentiveDet.Create(1, 1, 1001, 40m, 20000m, 1),
            GroupIncentiveDet.Create(2, 1, 1002, 35m, 17500m, 1),
            GroupIncentiveDet.Create(3, 1, 1003, 25m, 12500m, 1),
        };
        await context.GroupIncentiveDets.AddRangeAsync(details);

        await context.SaveChangesAsync();
        logger.LogInformation("Seeding completed.");
    }
}
