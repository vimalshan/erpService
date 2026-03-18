using Microsoft.EntityFrameworkCore;
using OrganizationSetup.Domain.Common;
using OrganizationSetup.Domain.Entities;
using OrganizationSetup.Infrastructure.Persistence.Configurations;

namespace OrganizationSetup.Infrastructure.Persistence;

public class OrganizationSetupDbContext : DbContext
{
    public OrganizationSetupDbContext(DbContextOptions<OrganizationSetupDbContext> options) : base(options) { }

    public DbSet<DealRole> DealRoles => Set<DealRole>();
    public DbSet<DealUserMap> DealUserMaps => Set<DealUserMap>();
    public DbSet<DealOrgParams> DealOrgParams => Set<DealOrgParams>();
    public DbSet<DealPpLimit> DealPpLimits => Set<DealPpLimit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new DealRoleConfiguration());
        modelBuilder.ApplyConfiguration(new DealUserMapConfiguration());
        modelBuilder.ApplyConfiguration(new DealOrgParamsConfiguration());
        modelBuilder.ApplyConfiguration(new DealPpLimitConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>().ToList();
        var result = await base.SaveChangesAsync(cancellationToken);
        
        // Domain events would be published here in a real UoW
        foreach (var entry in entries)
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
