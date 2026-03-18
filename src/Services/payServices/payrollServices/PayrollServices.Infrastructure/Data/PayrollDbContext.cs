using Microsoft.EntityFrameworkCore;
using PayrollServices.Domain.Entities;
using PayrollServices.Domain.Events;

namespace PayrollServices.Infrastructure.Data;

public class PayrollDbContext : DbContext
{
    public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options)
    {
    }

    public DbSet<PayrollBatch> PayrollBatches { get; set; } = null!;
    public DbSet<PayrollTransaction> PayrollTransactions { get; set; } = null!;
    public DbSet<PayrollAdjustment> PayrollAdjustments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ignore DomainEvent as it's not meant to be persisted
        modelBuilder.Ignore<DomainEvent>();

        // Apply entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PayrollDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
