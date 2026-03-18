using Microsoft.EntityFrameworkCore;
using OrganizationStructureService.Domain.Common;
using OrganizationStructureService.Domain.Entities;

namespace OrganizationStructureService.Infrastructure.Persistence;

public class OrganizationDbContext : DbContext
{
    public OrganizationDbContext(DbContextOptions<OrganizationDbContext> options) : base(options) { }

    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Division> Divisions => Set<Division>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Site> Sites => Set<Site>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Region> Regions => Set<Region>();
    public DbSet<Level> Levels => Set<Level>();
    public DbSet<HrRole> HrRoles => Set<HrRole>();
    public DbSet<LovMaster> LovMasters => Set<LovMaster>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrganizationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}
