using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MasterData.Domain.Entities;

#nullable enable

namespace MasterData.Infrastructure.Persistence
{
    /// <summary>
    /// Entity Framework Core DbContext for MasterData
    /// </summary>
    public class MasterDataDbContext : DbContext
    {
        public MasterDataDbContext(DbContextOptions<MasterDataDbContext> options) : base(options)
        {
        }

        public DbSet<CompanyUnitAggregate> CompanyUnits { get; set; } = null!;
        public DbSet<LocationAggregate> Locations { get; set; } = null!;
        public DbSet<SupplierAggregate> Suppliers { get; set; } = null!;
        public DbSet<StateAggregate> States { get; set; } = null!;
        public DbSet<CityAggregate> Cities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure CompanyUnitAggregate
            modelBuilder.Entity<CompanyUnitAggregate>(entity =>
            {
                entity.ToTable("COMPANY_UNITMASTER");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("COMPANY_UNIT_ID")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Code)
                    .HasColumnName("COMPANY_UNIT_CODE")
                    .HasMaxLength(3)
                    .IsRequired();
                entity.Property(e => e.Name)
                    .HasColumnName("COMPANY_UNIT_NAME")
                    .HasMaxLength(1000)
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("UpdatedAt");
                entity.Property(e => e.IsDeleted)
                    .HasColumnName("IsDeleted")
                    .HasDefaultValue(false);
            });

            // Configure LocationAggregate
            modelBuilder.Entity<LocationAggregate>(entity =>
            {
                entity.ToTable("LOCATION_MASTER");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("LOCATION_ID")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Name)
                    .HasColumnName("LOCATION_NAME")
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("UpdatedAt");
                entity.Property(e => e.IsDeleted)
                    .HasColumnName("IsDeleted")
                    .HasDefaultValue(false);
            });

            // Configure SupplierAggregate
            modelBuilder.Entity<SupplierAggregate>(entity =>
            {
                entity.ToTable("SUPPLIER_MASTER");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("SU_CUS_COD")
                    .HasMaxLength(25);
                entity.Ignore(e => e.Code);
                entity.Property(e => e.Name)
                    .HasColumnName("SU_CUS_NAM")
                    .HasMaxLength(100)
                    .IsRequired();
                entity.Property(e => e.Details)
                    .HasColumnName("SU_CUS_DET")
                    .HasMaxLength(200);
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("SU_ENT_DAT")
                    .IsRequired();
                entity.Property(e => e.EntryId)
                    .HasColumnName("SU_ENT_ID")
                    .HasMaxLength(25)
                    .IsRequired();
                entity.Property(e => e.EntryNumber)
                    .HasColumnName("SU_ENT_NUM")
                    .HasPrecision(38);
                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("UpdatedAt");
                entity.Property(e => e.IsDeleted)
                    .HasColumnName("IsDeleted")
                    .HasDefaultValue(false);
            });

            // Configure StateAggregate
            modelBuilder.Entity<StateAggregate>(entity =>
            {
                entity.ToTable("ORA_STATEMASTER");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("ORA_STATECODE")
                    .HasMaxLength(100);
                entity.Ignore(e => e.Code);
                entity.Property(e => e.Name)
                    .HasColumnName("ORA_STATENAME")
                    .HasMaxLength(200)
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("UpdatedAt");
                entity.Property(e => e.IsDeleted)
                    .HasColumnName("IsDeleted")
                    .HasDefaultValue(false);
            });

            // Configure CityAggregate
            modelBuilder.Entity<CityAggregate>(entity =>
            {
                entity.ToTable("ORA_CITYMASTER");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("ORA_CITYCODE")
                    .HasMaxLength(100);
                entity.Ignore(e => e.Code);
                entity.Property(e => e.Name)
                    .HasColumnName("ORA_CITYNAME")
                    .HasMaxLength(200)
                    .IsRequired();
                entity.Property(e => e.StateCode)
                    .HasColumnName("ORA_STATECODE")
                    .HasMaxLength(100)
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("UpdatedAt");
                entity.Property(e => e.IsDeleted)
                    .HasColumnName("IsDeleted")
                    .HasDefaultValue(false);
            });
        }
    }
}
