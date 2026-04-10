using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSCTransactional.Domain.Entities;

namespace SSCTransactional.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
    {
        try
        {
            await context.Database.EnsureCreatedAsync();

            if (!await context.DocumentStatuses.AnyAsync())
            {
                context.DocumentStatuses.AddRange(
                    DocumentStatus.Create("NW", "I", "New", "Pending New", stageOrder: 1),
                    DocumentStatus.Create("AL", "I", "Allocated", "Pending Allocation", stageOrder: 2),
                    DocumentStatus.Create("IP", "I", "In Processing", "Pending Processing", stageOrder: 3),
                    DocumentStatus.Create("VL", "I", "In Validation", "Pending Validation", stageOrder: 4),
                    DocumentStatus.Create("AP", "I", "Approved", "Pending Approval", stageOrder: 5),
                    DocumentStatus.Create("PY", "I", "Payment Pending", "Pending Payment", stageOrder: 6),
                    DocumentStatus.Create("CM", "I", "Completed", "Pending Completion", stageOrder: 7),
                    DocumentStatus.Create("RJ", "I", "Rejected", "Pending Rejection", stageOrder: 8),
                    DocumentStatus.Create("HD", "I", "On Hold", "Pending Hold", stageOrder: 9),
                    DocumentStatus.Create("RV", "I", "Revoked", "Pending Revoke", stageOrder: 10),
                    DocumentStatus.Create("DF", "I", "Defective", "Pending Defective", stageOrder: 11),
                    DocumentStatus.Create("RS", "I", "Rescan Requested", "Pending Rescan", stageOrder: 12)
                );

                await context.SaveChangesAsync();
                logger.LogInformation("[Seed] Seeded {Count} DocumentStatus records", 12);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[Seed] Error seeding database");
        }
    }
}
