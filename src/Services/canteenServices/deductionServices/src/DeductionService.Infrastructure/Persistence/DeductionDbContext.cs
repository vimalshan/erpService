using DeductionService.Domain.Entities;
using DeductionService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeductionService.Infrastructure.Persistence;

public class DeductionDbContext(DbContextOptions<DeductionDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<AdhocPayDeduction> AdhocPayDeductions => Set<AdhocPayDeduction>();
    public DbSet<AdhocPayDeductionHistory> AdhocPayDeductionHistories => Set<AdhocPayDeductionHistory>();
    public DbSet<DeductionAccess> DeductionAccesses => Set<DeductionAccess>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeductionDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await base.SaveChangesAsync(ct);
}
