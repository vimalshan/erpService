using AuthProvider.Domain.Entities;
using AuthProvider.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthProvider.Infrastructure.Data;

/// <summary>
/// Entity Framework Core DbContext for the AuthProvider service.
/// Connection: Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=AuthProviderDB
/// </summary>
public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Ignore<AuthProvider.Domain.Common.DomainEvent>();
        builder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
    }
}

// ─── EF Core Fluent API Configurations ───────────────────────────────────────

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users");
        b.HasKey(u => u.Id);
        b.Property(u => u.Id).ValueGeneratedNever();

        b.Property(u => u.Username).IsRequired().HasMaxLength(50);
        b.HasIndex(u => u.Username).IsUnique();

        // Email value object – owned type / converter
        b.Property(u => u.Email)
            .HasConversion(e => e.Value, v => Email.Create(v))
            .IsRequired().HasMaxLength(320);
        b.HasIndex(u => u.Email).IsUnique();

        // Password value object
        b.Property(u => u.PasswordHash)
            .HasConversion(p => p.Hash, h => Password.FromHash(h))
            .IsRequired().HasMaxLength(256);

        b.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        b.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        b.Property(u => u.CreatedAt).IsRequired();

        b.HasMany(u => u.UserRoles).WithOne(ur => ur.User).HasForeignKey(ur => ur.UserId);
        b.HasMany(u => u.RefreshTokens).WithOne().HasForeignKey(rt => rt.UserId);
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> b)
    {
        b.ToTable("Roles");
        b.HasKey(r => r.Id);
        b.Property(r => r.Id).ValueGeneratedNever();
        b.Property(r => r.Name).IsRequired().HasMaxLength(100);
        b.HasIndex(r => r.Name).IsUnique();
        b.Property(r => r.Description).HasMaxLength(500);

        b.HasMany(r => r.RolePermissions).WithOne(rp => rp.Role).HasForeignKey(rp => rp.RoleId);
    }
}

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> b)
    {
        b.ToTable("Permissions");
        b.HasKey(p => p.Id);
        b.Property(p => p.Id).ValueGeneratedNever();
        b.Property(p => p.Name).IsRequired().HasMaxLength(200);
        b.Property(p => p.Resource).IsRequired().HasMaxLength(100);
        b.Property(p => p.Action).IsRequired().HasMaxLength(50);
    }
}

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> b)
    {
        b.ToTable("UserRoles");
        b.HasKey(ur => new { ur.UserId, ur.RoleId });
        b.HasOne(ur => ur.Role).WithMany().HasForeignKey(ur => ur.RoleId);
    }
}

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> b)
    {
        b.ToTable("RolePermissions");
        b.HasKey(rp => new { rp.RoleId, rp.PermissionId });
        b.HasOne(rp => rp.Permission).WithMany().HasForeignKey(rp => rp.PermissionId);
    }
}

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> b)
    {
        b.ToTable("RefreshTokens");
        b.HasKey(rt => rt.Id);
        b.Property(rt => rt.Id).ValueGeneratedNever();
        b.Property(rt => rt.Token).IsRequired().HasMaxLength(500);
        b.HasIndex(rt => rt.Token);
        b.Property(rt => rt.CreatedByIp).HasMaxLength(50);
        b.Property(rt => rt.RevokedByIp).HasMaxLength(50);
    }
}

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> b)
    {
        b.ToTable("AuditLogs");
        b.HasKey(al => al.Id);
        b.Property(al => al.Id).ValueGeneratedNever();
        b.Property(al => al.Action).IsRequired().HasMaxLength(100);
        b.Property(al => al.Resource).IsRequired().HasMaxLength(200);
        b.Property(al => al.IpAddress).HasMaxLength(50);
        b.Property(al => al.Details).HasMaxLength(2000);
    }
}
