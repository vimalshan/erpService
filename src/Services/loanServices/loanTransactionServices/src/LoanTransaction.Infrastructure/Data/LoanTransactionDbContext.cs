using Microsoft.EntityFrameworkCore;
using LoanTransaction.Domain.Aggregates;
using LoanTransaction.Domain.Entities;
using LoanTransaction.Infrastructure.Data.Configurations;

namespace LoanTransaction.Infrastructure.Data;

public class LoanTransactionDbContext : DbContext
{
    public LoanTransactionDbContext(DbContextOptions<LoanTransactionDbContext> options) : base(options) { }

    public DbSet<LoanAggregate> Loans { get; set; } = null!;
    public DbSet<LoanInstallment> LoanInstallments { get; set; } = null!;
    public DbSet<LoanSettlement> LoanSettlements { get; set; } = null!;
    public DbSet<LoanLedger> LoanLedgers { get; set; } = null!;
    public DbSet<LoanEmpInterestRate> LoanEmpInterestRates { get; set; } = null!;
    public DbSet<LoanAdjustment> LoanAdjustments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new LoanMainConfiguration());
        modelBuilder.ApplyConfiguration(new LoanInstallmentConfiguration());
        modelBuilder.ApplyConfiguration(new LoanSettlementConfiguration());
        modelBuilder.ApplyConfiguration(new LoanLedgerConfiguration());
        modelBuilder.ApplyConfiguration(new LoanEmpInterestRateConfiguration());
        modelBuilder.ApplyConfiguration(new LoanAdjustmentConfiguration());
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed data is handled in migration seed SQL script
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var entry in ChangeTracker.Entries<Domain.Common.Entity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    break;
            }
        }
        return await base.SaveChangesAsync(ct);
    }
}
