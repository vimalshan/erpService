using Microsoft.EntityFrameworkCore;
using EligibilityService.Domain.Entities;

namespace EligibilityService.Infrastructure.Persistence;

public class EligibilityDbContext : DbContext
{
    public EligibilityDbContext(DbContextOptions<EligibilityDbContext> options) : base(options) { }

    public DbSet<EligibilityMaster> EligibilityMasters => Set<EligibilityMaster>();
    public DbSet<EligibilityMasterHistory> EligibilityMasterHistories => Set<EligibilityMasterHistory>();
    public DbSet<ShiftMapping> ShiftMappings => Set<ShiftMapping>();
    public DbSet<DaywiseEligibility> DaywiseEligibilities => Set<DaywiseEligibility>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EligibilityDbContext).Assembly);
    }
}
