using Microsoft.EntityFrameworkCore;
using TransactionProcessing.Domain.Entities;
using TransactionProcessing.Infrastructure.Persistence;

namespace TransactionProcessing.Infrastructure.Persistence.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(TransactionDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.TransactionBatches.AnyAsync()) return;

        var batch = TransactionBatch.Create("MANUAL", DateTime.UtcNow, 1);
        await context.TransactionBatches.AddAsync(batch);
        await context.SaveChangesAsync();

        var txn = FinancialTransaction.Create(
            "CASH_TRANSFER", "SEED", 1000m, 1, 1.0m, null,
            "System", null, "Seed transaction", 1);
        txn.AssignToBatch(batch.BatchId);
        await context.FinancialTransactions.AddAsync(txn);
        await context.SaveChangesAsync();

        txn.MarkProcessing(1);
        txn.MarkCompleted(1);

        batch.Complete(1, 0, 1000m);

        await context.SaveChangesAsync();
    }
}
