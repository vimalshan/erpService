using Microsoft.EntityFrameworkCore;
using LoanApplication.Domain.Aggregates;
using LoanApplication.Domain.Entities;
using LoanApplication.Domain.ValueObjects;
using LoanApplication.Infrastructure.Data.Configurations;

namespace LoanApplication.Infrastructure.Data;

/// <summary>
/// Loan Application DbContext
/// </summary>
public class LoanApplicationDbContext : DbContext
{
    public LoanApplicationDbContext(DbContextOptions<LoanApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<LoanApplicationAggregate> LoanApplications { get; set; } = null!;
    public DbSet<LoanAdditional> LoanAdditionals { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfiguration(new LoanApplicationConfiguration());
        modelBuilder.ApplyConfiguration(new LoanAdditionalConfiguration());

        // Seed initial data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Add seed data if needed
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update timestamps before saving
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not LoanApplication.Domain.Common.Entity entity)
                continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.ModifiedAt = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entity.ModifiedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
