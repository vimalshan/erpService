using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TdsService.Domain.Entities;
using TdsService.Domain.ValueObjects;

namespace TdsService.Infrastructure.Persistence.Configurations;

public sealed class TdsVendorConfiguration : IEntityTypeConfiguration<TdsVendor>
{
    public void Configure(EntityTypeBuilder<TdsVendor> builder)
    {
        builder.ToTable("TDS_VENDORS");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasColumnName("VENDOR_ID")
            .ValueGeneratedNever();

        builder.Property(v => v.VendorName)
            .HasColumnName("VENDOR_NAME")
            .HasMaxLength(240)
            .IsRequired();

        // EmailAddress value object — stored as VARCHAR(3000)
        builder.OwnsOne(v => v.EmailAddress, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("EMAIL_ADDRESS")
                .HasMaxLength(3000);
        });

        // PanNumber value object — stored as VARCHAR(30)
        builder.OwnsOne(v => v.PanNumber, pan =>
        {
            pan.Property(p => p.Value)
                .HasColumnName("PAN_NO")
                .HasMaxLength(30);
            pan.HasIndex(p => p.Value)
                .HasDatabaseName("IDX_TDS_VENDORS_PANNO");
        });

        // Ignore the navigation to TdsFile in this context (handled separately)
        builder.Ignore(v => v.Files);
    }
}
