using Microsoft.EntityFrameworkCore;
using AdminService.Domain.Entities;

namespace AdminService.Infrastructure.Persistence;

/// <summary>
/// Entity Framework Core database context
/// </summary>
public class AdminServiceDbContext : DbContext
{
    public AdminServiceDbContext(DbContextOptions<AdminServiceDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Admin units table
    /// </summary>
    public DbSet<AdminUnit> AdminUnits { get; set; } = null!;

    /// <summary>
    /// Admin access table
    /// </summary>
    public DbSet<AdminAccess> AdminAccess { get; set; } = null!;

    /// <summary>
    /// Admin contact table
    /// </summary>
    public DbSet<AdminContact> AdminContacts { get; set; } = null!;

    /// <summary>
    /// Finance units table
    /// </summary>
    public DbSet<FinanceUnit> FinanceUnits { get; set; } = null!;

    /// <summary>
    /// Finance access table
    /// </summary>
    public DbSet<FinanceAccess> FinanceAccess { get; set; } = null!;

    /// <summary>
    /// Route master table
    /// </summary>
    public DbSet<RouteMaster> RouteMasters { get; set; } = null!;

    /// <summary>
    /// Area master table
    /// </summary>
    public DbSet<AreaMaster> AreaMasters { get; set; } = null!;

    /// <summary>
    /// Area route map table
    /// </summary>
    public DbSet<AreaRouteMap> AreaRouteMaps { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure AdminUnit
        modelBuilder.Entity<AdminUnit>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.AdminType).HasMaxLength(1);
            entity.Property(e => e.UnitCode).HasMaxLength(3);
            entity.Property(e => e.ImageUrl).HasMaxLength(150);
            entity.HasMany(e => e.AccessConfigurations)
                .WithOne(a => a.AdminUnit)
                .HasForeignKey(a => a.AdminCode)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasMany(e => e.ContactDetails)
                .WithOne(c => c.AdminUnit)
                .HasForeignKey(c => c.AdminCode)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure AdminAccess
        modelBuilder.Entity<AdminAccess>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CompanyCode).HasMaxLength(3);
            entity.Property(e => e.LocalUserCode).HasMaxLength(20);
            entity.Property(e => e.ContactEmail).HasMaxLength(100);
        });

        // Configure AdminContact
        modelBuilder.Entity<AdminContact>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).HasMaxLength(50);
            entity.Property(e => e.Phone1).HasMaxLength(50);
            entity.Property(e => e.Phone2).HasMaxLength(50);
            entity.Property(e => e.ContactType).HasMaxLength(50);
        });

        // Configure FinanceUnit
        modelBuilder.Entity<FinanceUnit>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.UnitCode).HasMaxLength(3);
            entity.Property(e => e.LocationOption).HasMaxLength(1);
            entity.HasMany(e => e.AccessConfigurations)
                .WithOne(a => a.FinanceUnit)
                .HasForeignKey(a => a.UnitId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure FinanceAccess
        modelBuilder.Entity<FinanceAccess>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).HasMaxLength(20);
            entity.Property(e => e.EmailId).HasMaxLength(30);
        });

        // Configure RouteMaster
        modelBuilder.Entity<RouteMaster>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RouteName).IsRequired().HasMaxLength(200);
            entity.HasMany(e => e.AreaMappings)
                .WithOne(m => m.RouteMaster)
                .HasForeignKey(m => m.RouteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure AreaMaster
        modelBuilder.Entity<AreaMaster>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AreaName).IsRequired().HasMaxLength(200);
            entity.HasMany(e => e.RouteMappings)
                .WithOne(m => m.AreaMaster)
                .HasForeignKey(m => m.AreaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure AreaRouteMap
        modelBuilder.Entity<AreaRouteMap>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.RouteMaster)
                .WithMany(r => r.AreaMappings)
                .HasForeignKey(e => e.RouteId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.AreaMaster)
                .WithMany(a => a.RouteMappings)
                .HasForeignKey(e => e.AreaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Global query filters for soft delete
        modelBuilder.Entity<AdminUnit>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<AdminAccess>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<AdminContact>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<FinanceUnit>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<FinanceAccess>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<RouteMaster>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<AreaMaster>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<AreaRouteMap>().HasQueryFilter(e => !e.IsDeleted);
    }
}
