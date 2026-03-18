using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VendorService.Domain.Entities;
using VendorService.Domain.ValueObjects;

namespace VendorService.Infrastructure.Data.Configurations;

internal sealed class VendorMasterConfiguration : IEntityTypeConfiguration<VendorMaster>
{
    public void Configure(EntityTypeBuilder<VendorMaster> builder)
    {
        builder.ToTable("VENDOR_MASTER");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasColumnName("VM_ID")
            .ValueGeneratedNever();

        builder.Property(v => v.CategoryId)
            .HasColumnName("VM_CATID")
            .IsRequired();

        builder.Property(v => v.LocationId)
            .HasColumnName("VM_LOC_ID")
            .IsRequired();

        builder.Property(v => v.Name)
            .HasColumnName("VM_NAME")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(n => n.Value, v => new VendorName(v));

        builder.Property(v => v.Email)
            .HasColumnName("VM_EMAIL")
            .HasMaxLength(50)
            .HasConversion(
                e => e == null ? null : e.Value,
                v => v == null ? null : new Email(v));

        builder.Property(v => v.Address)
            .HasColumnName("VM_ADDRESS")
            .HasMaxLength(200)
            .IsRequired()
            .HasConversion(a => a.Value, v => new Address(v));

        builder.Property(v => v.UpdatedBy)
            .HasColumnName("VM_UPDATED_BY")
            .IsRequired();

        builder.Property(v => v.UpdatedOn)
            .HasColumnName("VM_UPDATED_ON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(v => v.LiveStatus)
            .HasColumnName("VM_LIVESTATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(ls => ls.Value.ToString(), v => new LiveStatus(v[0]));

        builder.HasIndex(v => v.LocationId).HasDatabaseName("IDX_VENDOR_MASTER_LOCID");
        builder.HasIndex(v => v.LiveStatus).HasDatabaseName("IDX_VENDOR_MASTER_STATUS");

        builder.Ignore(v => v.DomainEvents);
    }
}
