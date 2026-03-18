using ContributionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContributionService.Infrastructure.Persistence;

public static class ContributionDbSeeder
{
    public static async Task SeedAsync(ContributionDbContext context)
    {
        if (await context.SuperannuationTrustNames.AnyAsync())
            return;

        context.SuperannuationTrustNames.AddRange(
            new SuperannuationTrustName { StFndNum = 1, StFndNam = "Default PF Trust" },
            new SuperannuationTrustName { StFndNum = 2, StFndNam = "Superannuation Trust A" },
            new SuperannuationTrustName { StFndNum = 3, StFndNam = "Superannuation Trust B" }
        );

        if (!await context.ContributionMain.AnyAsync())
        {
            var batch = ContributionMain.Create(0, "DFL", "REG", "001",
                new DateTime(2026, 1, 1), new DateTime(2026, 1, 31), 1);

            context.ContributionMain.Add(batch);
        }

        context.ContributionProcessLogs.Add(
            ContributionProcessLog.Create("SEED", "Database seeded successfully", 0));

        await context.SaveChangesAsync();
    }
}
