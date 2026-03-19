using MasterDataService.Domain.Entities;
using MasterDataService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MasterDataService.Infrastructure.Persistence.EfCore;

public class MasterDataDbContext(DbContextOptions<MasterDataDbContext> options) : DbContext(options)
{
    public DbSet<LovMaster> LovMasters => Set<LovMaster>();
    public DbSet<LovTypeMaster> LovTypeMasters => Set<LovTypeMaster>();
    public DbSet<HoldTypeMaster> HoldTypeMasters => Set<HoldTypeMaster>();
    public DbSet<LocationScanParam> LocationScanParams => Set<LocationScanParam>();
    public DbSet<ScannerMaster> ScannerMasters => Set<ScannerMaster>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // LOV_MAST
        modelBuilder.Entity<LovMaster>(entity =>
        {
            entity.ToTable("LOV_MAST");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("LOV_ID").ValueGeneratedNever();
            entity.Property(e => e.LovType).HasColumnName("LOV_TYPE").HasMaxLength(10).IsRequired();
            entity.Property(e => e.LovName).HasColumnName("LOV_NAME").HasMaxLength(200).IsRequired();
            entity.Ignore(e => e.CreatedAt);
            entity.Ignore(e => e.CreatedBy);
            entity.Ignore(e => e.LastModifiedAt);
            entity.Ignore(e => e.LastModifiedBy);
            entity.Ignore(e => e.DomainEvents);
        });

        // LOV_TYPEMAST
        modelBuilder.Entity<LovTypeMaster>(entity =>
        {
            entity.ToTable("LOV_TYPEMAST");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("LOV_TYPECODE").HasMaxLength(10);
            entity.Property(e => e.LovTypeName).HasColumnName("LOV_TYPENAME").HasMaxLength(50).IsRequired();
            entity.Ignore(e => e.CreatedAt);
            entity.Ignore(e => e.CreatedBy);
            entity.Ignore(e => e.LastModifiedAt);
            entity.Ignore(e => e.LastModifiedBy);
            entity.Ignore(e => e.DomainEvents);
        });

        // HOLDTYPE_MAST
        modelBuilder.Entity<HoldTypeMaster>(entity =>
        {
            entity.ToTable("HOLDTYPE_MAST");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("HOLD_ID").ValueGeneratedNever();
            entity.Property(e => e.HoldName).HasColumnName("HOLD_NAME").HasMaxLength(100);
            entity.Property(e => e.HoldCategory).HasColumnName("HOLD_CATEGORY").HasColumnType("char(1)");
            entity.Ignore(e => e.CreatedAt);
            entity.Ignore(e => e.CreatedBy);
            entity.Ignore(e => e.LastModifiedAt);
            entity.Ignore(e => e.LastModifiedBy);
            entity.Ignore(e => e.DomainEvents);
        });

        // LOCATION_SCANPARAMS
        modelBuilder.Entity<LocationScanParam>(entity =>
        {
            entity.ToTable("LOCATION_SCANPARAMS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("LOCSCANPARAM_ID").ValueGeneratedNever();
            entity.Property(e => e.LocationId).HasColumnName("LOC_ID").IsRequired();
            entity.OwnsOne(e => e.EffectivePeriod, ep =>
            {
                ep.Property(p => p.EffectiveDate).HasColumnName("LOC_EFFDATE").HasColumnType("datetime2(3)").IsRequired();
                ep.Property(p => p.ClosingDate).HasColumnName("LOC_CLSDATE").HasColumnType("datetime2(3)");
            });
            entity.Ignore(e => e.CreatedAt);
            entity.Ignore(e => e.CreatedBy);
            entity.Ignore(e => e.LastModifiedAt);
            entity.Ignore(e => e.LastModifiedBy);
            entity.Ignore(e => e.DomainEvents);
        });

        // SCANNER_MASTER
        modelBuilder.Entity<ScannerMaster>(entity =>
        {
            entity.ToTable("SCANNER_MASTER");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("DEVICE_ID").ValueGeneratedNever();
            entity.Property(e => e.DeviceName).HasColumnName("DEVICE_NAME").HasMaxLength(100);
            entity.Property(e => e.DeviceLocationId).HasColumnName("DEVICE_LOCID").IsRequired();
            entity.OwnsOne(e => e.DevicePath, dp =>
            {
                dp.Property(p => p.Value).HasColumnName("DEVICE_PATH").HasMaxLength(1000);
            });
            entity.Ignore(e => e.CreatedAt);
            entity.Ignore(e => e.CreatedBy);
            entity.Ignore(e => e.LastModifiedAt);
            entity.Ignore(e => e.LastModifiedBy);
            entity.Ignore(e => e.DomainEvents);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LovTypeMaster>().HasData(
            LovTypeMaster.Create("STATUS", "Status Codes"),
            LovTypeMaster.Create("CATEGORY", "Category Codes"),
            LovTypeMaster.Create("PRIORITY", "Priority Levels")
        );

        modelBuilder.Entity<LovMaster>().HasData(
            LovMaster.Create(1, "STATUS", "Active"),
            LovMaster.Create(2, "STATUS", "Inactive"),
            LovMaster.Create(3, "STATUS", "Pending"),
            LovMaster.Create(4, "CATEGORY", "General"),
            LovMaster.Create(5, "CATEGORY", "Special"),
            LovMaster.Create(6, "PRIORITY", "High"),
            LovMaster.Create(7, "PRIORITY", "Medium"),
            LovMaster.Create(8, "PRIORITY", "Low")
        );

        modelBuilder.Entity<HoldTypeMaster>().HasData(
            HoldTypeMaster.Create(1, "Quality Hold", "Q"),
            HoldTypeMaster.Create(2, "Safety Hold", "S"),
            HoldTypeMaster.Create(3, "Financial Hold", "F")
        );
    }
}
