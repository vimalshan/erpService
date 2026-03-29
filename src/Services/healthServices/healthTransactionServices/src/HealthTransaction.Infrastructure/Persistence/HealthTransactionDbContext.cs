using HealthTransaction.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthTransaction.Infrastructure.Persistence;

public class HealthTransactionDbContext : DbContext
{
    public HealthTransactionDbContext(DbContextOptions<HealthTransactionDbContext> options)
        : base(options) { }

    public DbSet<PreEmploymentCheckup> PreEmploymentCheckups => Set<PreEmploymentCheckup>();
    public DbSet<PfiHistory> PfiHistories => Set<PfiHistory>();
    public DbSet<CheckupCard> CheckupCards => Set<CheckupCard>();
    public DbSet<CheckupCardSub> CheckupCardSubs => Set<CheckupCardSub>();
    public DbSet<DynamicHealthDetail> DynamicHealthDetails => Set<DynamicHealthDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HealthTransactionDbContext).Assembly);

        // Ignore domain events on all mapped entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.GetProperty("DomainEvents") != null)
                modelBuilder.Entity(entityType.ClrType).Ignore("DomainEvents");
        }

        base.OnModelCreating(modelBuilder);
    }
}
