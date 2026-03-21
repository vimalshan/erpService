using MediatR;
using Microsoft.EntityFrameworkCore;
using SecurityService.Domain.Common;
using SecurityService.Domain.Entities;

namespace SecurityService.Infrastructure.Persistence;

public class SecurityDbContext : DbContext
{
    private readonly IMediator _mediator;

    public SecurityDbContext(DbContextOptions<SecurityDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.UserId);
            e.Property(u => u.UserId).ValueGeneratedOnAdd();
            e.Property(u => u.Username).HasMaxLength(50).IsRequired();
            e.HasIndex(u => u.Username).IsUnique();
            e.Property(u => u.PasswordHash).HasMaxLength(256).IsRequired();
            e.Property(u => u.Email).HasMaxLength(100).IsRequired();
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.FullName).HasMaxLength(100).IsRequired();
            e.Property(u => u.IsActive).HasDefaultValue(true);
            e.Property(u => u.CreatedDate).HasDefaultValueSql("GETDATE()");
            e.Ignore(u => u.DomainEvents);
        });

        // Role
        modelBuilder.Entity<Role>(e =>
        {
            e.HasKey(r => r.RoleId);
            e.Property(r => r.RoleId).ValueGeneratedOnAdd();
            e.Property(r => r.RoleName).HasMaxLength(50).IsRequired();
            e.HasIndex(r => r.RoleName).IsUnique();
            e.Property(r => r.Description).HasMaxLength(255);
            e.Ignore(r => r.DomainEvents);
        });

        // Permission
        modelBuilder.Entity<Permission>(e =>
        {
            e.HasKey(p => p.PermissionId);
            e.Property(p => p.PermissionId).ValueGeneratedOnAdd();
            e.Property(p => p.PermissionName).HasMaxLength(100).IsRequired();
            e.HasIndex(p => p.PermissionName).IsUnique();
            e.Property(p => p.Module).HasMaxLength(50);
            e.Property(p => p.Description).HasMaxLength(255);
            e.Ignore(p => p.DomainEvents);
        });

        // RolePermission (many-to-many)
        modelBuilder.Entity<RolePermission>(e =>
        {
            e.HasKey(rp => new { rp.RoleId, rp.PermissionId });
            e.HasOne(rp => rp.Role).WithMany(r => r.RolePermissions).HasForeignKey(rp => rp.RoleId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(rp => rp.Permission).WithMany(p => p.RolePermissions).HasForeignKey(rp => rp.PermissionId).OnDelete(DeleteBehavior.Cascade);
        });

        // UserRole (many-to-many)
        modelBuilder.Entity<UserRole>(e =>
        {
            e.HasKey(ur => new { ur.UserId, ur.RoleId });
            e.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, RoleName = "Admin", Description = "System Administrator" },
            new Role { RoleId = 2, RoleName = "Manager", Description = "Manager role" },
            new Role { RoleId = 3, RoleName = "User", Description = "Standard user role" }
        );

        modelBuilder.Entity<Permission>().HasData(
            new Permission { PermissionId = 1, PermissionName = "Users.View", Module = "Security", Description = "View users" },
            new Permission { PermissionId = 2, PermissionName = "Users.Create", Module = "Security", Description = "Create users" },
            new Permission { PermissionId = 3, PermissionName = "Users.Edit", Module = "Security", Description = "Edit users" },
            new Permission { PermissionId = 4, PermissionName = "Users.Delete", Module = "Security", Description = "Delete users" },
            new Permission { PermissionId = 5, PermissionName = "Roles.View", Module = "Security", Description = "View roles" },
            new Permission { PermissionId = 6, PermissionName = "Roles.Create", Module = "Security", Description = "Create roles" },
            new Permission { PermissionId = 7, PermissionName = "Roles.Edit", Module = "Security", Description = "Edit roles" },
            new Permission { PermissionId = 8, PermissionName = "Roles.Delete", Module = "Security", Description = "Delete roles" },
            new Permission { PermissionId = 9, PermissionName = "Permissions.View", Module = "Security", Description = "View permissions" },
            new Permission { PermissionId = 10, PermissionName = "Permissions.Manage", Module = "Security", Description = "Manage permissions" }
        );

        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = 1, PermissionId = 1 },
            new RolePermission { RoleId = 1, PermissionId = 2 },
            new RolePermission { RoleId = 1, PermissionId = 3 },
            new RolePermission { RoleId = 1, PermissionId = 4 },
            new RolePermission { RoleId = 1, PermissionId = 5 },
            new RolePermission { RoleId = 1, PermissionId = 6 },
            new RolePermission { RoleId = 1, PermissionId = 7 },
            new RolePermission { RoleId = 1, PermissionId = 8 },
            new RolePermission { RoleId = 1, PermissionId = 9 },
            new RolePermission { RoleId = 1, PermissionId = 10 },
            new RolePermission { RoleId = 2, PermissionId = 1 },
            new RolePermission { RoleId = 2, PermissionId = 5 },
            new RolePermission { RoleId = 2, PermissionId = 9 },
            new RolePermission { RoleId = 3, PermissionId = 1 },
            new RolePermission { RoleId = 3, PermissionId = 5 }
        );
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Dispatch domain events before saving
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();

        foreach (var entity in entities)
            entity.ClearDomainEvents();

        var result = await base.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, ct);

        return result;
    }
}
