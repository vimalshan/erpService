using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VendorService.Domain.Entities;

namespace VendorService.Infrastructure.Data.Configurations;

internal sealed class TdsVendorConfiguration : IEntityTypeConfiguration<TdsVendor>
{
    public void Configure(EntityTypeBuilder<TdsVendor> builder)
    {
        builder.ToTable("TDS_VENDORS");

        // No primary key defined in DB; use EF shadow key
        builder.HasNoKey();

        builder.Property(v => v.VendorId).HasColumnName("VENDOR_ID");
        builder.Property(v => v.VendorName).HasColumnName("VENDOR_NAME").HasMaxLength(240);
        builder.Property(v => v.EmailAddress).HasColumnName("EMAIL_ADDRESS").HasMaxLength(3000);
        builder.Property(v => v.PanNo).HasColumnName("PAN_NO").HasMaxLength(30);

        builder.Ignore(v => v.DomainEvents);
    }
}
