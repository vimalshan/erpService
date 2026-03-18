using GroupIncentiveService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GroupIncentiveService.Infrastructure.Persistence;

public class GroupIncentiveDbContext : DbContext
{
    public GroupIncentiveDbContext(DbContextOptions<GroupIncentiveDbContext> options)
        : base(options) { }

    public DbSet<GroupMaster> GroupMasters => Set<GroupMaster>();
    public DbSet<GroupEmployeeMap> GroupEmployeeMaps => Set<GroupEmployeeMap>();
    public DbSet<GroupIncentiveMain> GroupIncentiveMains => Set<GroupIncentiveMain>();
    public DbSet<GroupIncentiveDet> GroupIncentiveDets => Set<GroupIncentiveDet>();
    public DbSet<GroupIncentiveBreak> GroupIncentiveBreaks => Set<GroupIncentiveBreak>();
    public DbSet<GroupIncentiveApproval> GroupIncentiveApprovals => Set<GroupIncentiveApproval>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GroupIncentiveDbContext).Assembly);
    }
}
