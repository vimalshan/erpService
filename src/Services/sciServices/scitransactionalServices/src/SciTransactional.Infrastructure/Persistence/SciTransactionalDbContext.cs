using Microsoft.EntityFrameworkCore;
using SciTransactional.Domain.Entities;

namespace SciTransactional.Infrastructure.Persistence;

public sealed class SciTransactionalDbContext(DbContextOptions<SciTransactionalDbContext> options)
    : DbContext(options)
{
    public DbSet<SparshNavigationEntity> SparshNavigations => Set<SparshNavigationEntity>();
    public DbSet<NormsMainEntity> NormsMain => Set<NormsMainEntity>();
    public DbSet<NormsMasterEntity> NormsMaster => Set<NormsMasterEntity>();
    public DbSet<AdvanceLicenseEntity> AdvanceLicenses => Set<AdvanceLicenseEntity>();
    public DbSet<AdvanceLicenseEntitlementEntity> AdvanceLicenseEntitlements => Set<AdvanceLicenseEntitlementEntity>();
    public DbSet<AutoMailStatusEntity> AutoMailStatuses => Set<AutoMailStatusEntity>();
    public DbSet<AutoMailIdEntity> AutoMailIds => Set<AutoMailIdEntity>();
    public DbSet<ActualOrderMapEntity> ActualOrderMaps => Set<ActualOrderMapEntity>();
    public DbSet<VehicleDirectEntryEntity> VehicleDirectEntries => Set<VehicleDirectEntryEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SciTransactionalDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
