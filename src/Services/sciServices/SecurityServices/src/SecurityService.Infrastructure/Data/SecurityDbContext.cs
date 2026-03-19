using Microsoft.EntityFrameworkCore;
using SecurityService.Domain.Entities;

namespace SecurityService.Infrastructure.Data;

public class SecurityDbContext : DbContext
{
    public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<AccessRole> AccessRoles { get; set; } = null!;
    public DbSet<AccessRoleMaster> AccessRoleMasters { get; set; } = null!;
    public DbSet<AccessRoleMenu> AccessRoleMenus { get; set; } = null!;
    public DbSet<MenuMaster> MenuMasters { get; set; } = null!;
    public DbSet<UserMasterMap> UserMasterMaps { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SecurityDbContext).Assembly);
    }
}
