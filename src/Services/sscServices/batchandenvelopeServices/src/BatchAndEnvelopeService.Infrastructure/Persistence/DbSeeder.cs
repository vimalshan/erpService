using Microsoft.EntityFrameworkCore;
using BatchAndEnvelopeService.Domain.Aggregates;
using BatchAndEnvelopeService.Domain.Entities;

namespace BatchAndEnvelopeService.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.ScanLotMasters.AnyAsync())
        {
            var lot = ScanLotMaster.Create(1001, 1, 100, 5001);
            await context.ScanLotMasters.AddAsync(lot);
        }

        if (!await context.Envelopes.AnyAsync())
        {
            var env1 = EnvelopeAggregate.Create(1, "REG", 1, 101);
            env1.AddDetail(EnvelopeDetail.Create(1, 1, "REG", 2001, 1));
            env1.AddDetail(EnvelopeDetail.Create(2, 1, "REG", 2002, 1));
            await context.Envelopes.AddAsync(env1);

            var env2 = EnvelopeAggregate.Create(2, "EXP", 1, 101);
            env2.AddDetail(EnvelopeDetail.Create(3, 2, "EXP", 2003, 1));
            await context.Envelopes.AddAsync(env2);
        }

        if (!await context.Batches.AnyAsync())
        {
            var batch = BatchAggregate.Create(10001, 1, 101, 1, "POD-2026-001", "FastCourier");
            batch.AddDetail(BatchDetail.Create(1, 10001, 1, 1));
            batch.AddDetail(BatchDetail.Create(2, 10001, 2, 1));
            await context.Batches.AddAsync(batch);
        }

        foreach (var entity in context.ChangeTracker.Entries())
            entity.Entity.GetType().GetMethod("ClearDomainEvents")?.Invoke(entity.Entity, null);

        await context.SaveChangesAsync();
    }
}
