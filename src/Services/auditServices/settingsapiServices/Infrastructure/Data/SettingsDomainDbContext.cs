using SettingsService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SettingsService.Infrastructure.Data;

public class SettingsDomainDbContext : DbContext
{
    public SettingsDomainDbContext(DbContextOptions<SettingsDomainDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<UserPreference> UserPreferences => Set<UserPreference>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users"); e.HasKey(x => x.UserId);
            e.Property(x => x.Username).HasMaxLength(100).IsRequired();
            e.Property(x => x.Email).HasMaxLength(255).IsRequired();
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.PasswordHash).HasMaxLength(255).IsRequired();
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.ModifiedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.Phone).HasMaxLength(20);
            e.Property(x => x.Position).HasMaxLength(100);
            e.Property(x => x.Department).HasMaxLength(100);
            e.Property(x => x.TimeZone).HasMaxLength(50).HasDefaultValue("UTC");
            e.Property(x => x.Language).HasMaxLength(10).HasDefaultValue("EN");
            e.Property(x => x.IsEmailVerified).HasDefaultValue(false);
            e.Property(x => x.EmailVerificationToken).HasMaxLength(255);
            e.Property(x => x.PasswordResetToken).HasMaxLength(255);
            e.Property(x => x.TwoFactorEnabled).HasDefaultValue(false);
            e.Property(x => x.TwoFactorSecret).HasMaxLength(100);
            e.HasIndex(x => x.Username).IsUnique();
            e.HasIndex(x => x.Email).IsUnique();
            e.HasIndex(x => x.IsActive); e.HasIndex(x => x.LastLoginDate);
            e.Ignore(x => x.DomainEvents);
            e.HasMany(x => x.UserRoles).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            e.HasMany(x => x.Preferences).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Role>(e =>
        {
            e.ToTable("Roles"); e.HasKey(x => x.RoleId);
            e.Property(x => x.RoleName).HasMaxLength(100).IsRequired();
            e.Property(x => x.RoleCode).HasMaxLength(50).IsRequired();
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.ModifiedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.IsSystemRole).HasDefaultValue(false);
            e.HasIndex(x => x.RoleName).IsUnique();
            e.HasIndex(x => x.RoleCode).IsUnique();
            e.HasIndex(x => x.IsActive);
            e.HasMany(x => x.UserRoles).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);
        });

        modelBuilder.Entity<UserRole>(e =>
        {
            e.ToTable("UserRoles"); e.HasKey(x => x.UserRoleId);
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.ModifiedDate).HasDefaultValueSql("GETDATE()");
            e.HasIndex(x => new { x.UserId, x.RoleId }).IsUnique();
            e.HasIndex(x => x.UserId); e.HasIndex(x => x.RoleId); e.HasIndex(x => x.IsActive);
        });

        modelBuilder.Entity<UserPreference>(e =>
        {
            e.ToTable("UserPreferences"); e.HasKey(x => x.UserPreferenceId);
            e.Property(x => x.PreferenceKey).HasMaxLength(100).IsRequired();
            e.Property(x => x.PreferenceType).HasMaxLength(50).HasDefaultValue("String");
            e.Property(x => x.Category).HasMaxLength(50);
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.ModifiedDate).HasDefaultValueSql("GETDATE()");
            e.HasIndex(x => new { x.UserId, x.PreferenceKey }).IsUnique();
            e.HasIndex(x => x.UserId); e.HasIndex(x => x.PreferenceKey);
            e.HasIndex(x => x.Category); e.HasIndex(x => x.IsActive);
        });
    }
}
