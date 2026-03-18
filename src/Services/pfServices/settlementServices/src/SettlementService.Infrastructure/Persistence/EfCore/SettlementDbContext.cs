using Microsoft.EntityFrameworkCore;
using SettlementService.Domain.Aggregates;
using SettlementService.Domain.Entities;

namespace SettlementService.Infrastructure.Persistence.EfCore;

public class SettlementDbContext : DbContext
{
    public DbSet<Settlement> Settlements => Set<Settlement>();
    public DbSet<SettlementDeduction> SettlementDeductions => Set<SettlementDeduction>();
    public DbSet<SettlementApproval> SettlementApprovals => Set<SettlementApproval>();
    public DbSet<SettlementPayment> SettlementPayments => Set<SettlementPayment>();

    public SettlementDbContext(DbContextOptions<SettlementDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SettlementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
