using BatchService.Domain.Entities;
using BatchService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BatchService.Infrastructure.Seeding;

/// <summary>Seeds initial BATCH_MASTER rows if the table is empty.</summary>
public static class BatchDataSeeder
{
    public static async Task SeedAsync(BatchDbContext context, CancellationToken ct = default)
    {
        if (await context.BatchMasters.AnyAsync(ct))
            return;

        var seeds = new[]
        {
            BatchMaster.Create(1001, 1, 1),
            BatchMaster.Create(1002, 2, 1),
            BatchMaster.Create(1003, 3, 1)
        };

        foreach (var seed in seeds)
            seed.ClearDomainEvents();   // no event publishing during seed

        await context.BatchMasters.AddRangeAsync(seeds, ct);
        await context.SaveChangesAsync(ct);
    }
}
