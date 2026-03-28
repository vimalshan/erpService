using Microsoft.EntityFrameworkCore;
using AuthorizationService.Domain.Entities;

namespace AuthorizationService.Infrastructure.Data;

public class AuthorizationDbContext : DbContext
{
    public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Right> Rights { get; set; } = null!;
    public DbSet<SpecialInput> SpecialInputs { get; set; } = null!;
    public DbSet<SpecialInputMaster> SpecialInputMasters { get; set; } = null!;
    public DbSet<TrackerRight> TrackerRights { get; set; } = null!;
    public DbSet<UserRight> UserRights { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureRightEntity(modelBuilder);
        ConfigureSpecialInputEntity(modelBuilder);
        ConfigureSpecialInputMasterEntity(modelBuilder);
        ConfigureTrackerRightEntity(modelBuilder);
        ConfigureUserRightEntity(modelBuilder);
    }

    private static void ConfigureRightEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Right>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RightCode).IsRequired().HasColumnType("decimal(38,0)");
            entity.Property(e => e.RightDescription).HasMaxLength(3);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.RightCode).IsUnique();
        });
    }

    private static void ConfigureSpecialInputEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SpecialInput>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SpecialInputId).IsRequired().HasColumnType("decimal(38,0)");
            entity.Property(e => e.YearId).IsRequired().HasColumnType("decimal(38,0)");
            entity.Property(e => e.RoleType).IsRequired().HasMaxLength(10);
            entity.Property(e => e.EmployeeSysId).IsRequired().HasColumnType("decimal(38,0)");
            entity.Property(e => e.AppraisalSysId).IsRequired().HasColumnType("decimal(38,0)");
            entity.Property(e => e.Inputs).IsRequired().HasMaxLength(4000);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(1);
            entity.Property(e => e.CreatedOn).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.SpecialInputId);
            entity.HasIndex(e => e.EmployeeSysId);
        });
    }

    private static void ConfigureSpecialInputMasterEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SpecialInputMaster>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SpecialInputId).IsRequired().HasColumnType("decimal(38,0)");
            entity.Property(e => e.YearId).IsRequired().HasColumnType("decimal(38,0)");
            entity.Property(e => e.RoleType).IsRequired().HasMaxLength(10);
            entity.Property(e => e.EmployeeSysId).IsRequired().HasColumnType("decimal(38,0)");
            entity.Property(e => e.AppraisalSysId).IsRequired().HasColumnType("decimal(38,0)");
            entity.Property(e => e.CreatedBy).IsRequired().HasColumnType("decimal(38,0)");
            entity.Property(e => e.CreatedOn).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.SpecialInputId);
        });
    }

    private static void ConfigureTrackerRightEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrackerRight>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired().HasMaxLength(65);
            entity.Property(e => e.PinNumber).HasColumnType("decimal(38,0)");
            entity.Property(e => e.TrackerMode).HasMaxLength(3);
            entity.Property(e => e.BusinessCode).HasMaxLength(9);
            entity.Property(e => e.UnitCode).HasMaxLength(3);
            entity.Property(e => e.TrackerRights).HasMaxLength(1);
            entity.Property(e => e.VtcRights).HasMaxLength(1);
            entity.Property(e => e.RepresentingUnit).HasMaxLength(1);
            entity.Property(e => e.LetRight).HasMaxLength(1);
            entity.Property(e => e.CarRight).HasMaxLength(1);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.BusinessCode);
        });
    }

    private static void ConfigureUserRightEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRight>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).HasMaxLength(60);
            entity.Property(e => e.PinNumber).HasColumnType("decimal(38,0)");
            entity.Property(e => e.RightCode).HasColumnType("decimal(38,0)");
            entity.Property(e => e.BusinessCode).HasMaxLength(9);
            entity.Property(e => e.UnitCode).HasMaxLength(6);
            entity.Property(e => e.RightMode).HasColumnType("decimal(38,0)");
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.RightCode);
        });
    }
}
