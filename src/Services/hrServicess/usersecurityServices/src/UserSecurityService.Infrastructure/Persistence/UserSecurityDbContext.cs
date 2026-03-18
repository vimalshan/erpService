using Microsoft.EntityFrameworkCore;
using UserSecurityService.Domain.Entities;
using UserSecurityService.Domain.Interfaces;

namespace UserSecurityService.Infrastructure.Persistence;

public class UserSecurityDbContext : DbContext, IUnitOfWork
{
    public UserSecurityDbContext(DbContextOptions<UserSecurityDbContext> options) : base(options) { }

    public DbSet<UserProfilePfs> UserProfiles => Set<UserProfilePfs>();
    public DbSet<UserAppsMap> UserAppsMappings => Set<UserAppsMap>();
    public DbSet<UserCalenderMap> UserCalenderMaps => Set<UserCalenderMap>();
    public DbSet<UserMenuMap> UserMenuMaps => Set<UserMenuMap>();
    public DbSet<UserUnitMap> UserUnitMaps => Set<UserUnitMap>();
    public DbSet<UserUnitMapLog> UserUnitMapLogs => Set<UserUnitMapLog>();
    public DbSet<UserCalenderMapLog> UserCalenderMapLogs => Set<UserCalenderMapLog>();
    public DbSet<UserMenuMapLog> UserMenuMapLogs => Set<UserMenuMapLog>();
    public DbSet<EmpPasswordChange> EmpPasswordChanges => Set<EmpPasswordChange>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserSecurityDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await base.SaveChangesAsync(ct);
}
