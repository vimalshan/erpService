using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustService.Domain.Entities;

namespace TrustService.Infrastructure.Persistence.Configurations;

public class TrustMasterConfiguration : IEntityTypeConfiguration<TrustMaster>
{
    public void Configure(EntityTypeBuilder<TrustMaster> builder)
    {
        builder.ToTable("TRUST_MASTER");

        builder.HasKey(t => t.TrustCode);

        builder.Property(t => t.TrustCode).HasColumnName("TRUST_CODE").HasMaxLength(3).IsFixedLength();
        builder.Property(t => t.TrustShortName).HasColumnName("TRUST_SHORT_NAME").HasMaxLength(65).IsRequired();
        builder.Property(t => t.TrustType).HasColumnName("TRUST_TYPE").HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(t => t.TrustStartDate).HasColumnName("TRUST_START_DATE").HasPrecision(3).IsRequired();
        builder.Property(t => t.TrustClosureDate).HasColumnName("TRUST_CLOSURE_DATE").HasPrecision(3);
        builder.Property(t => t.TrustId).HasColumnName("TRUST_ID").HasMaxLength(65);
        builder.Property(t => t.AddressLine1).HasColumnName("ADDRESS_LINE1").HasMaxLength(200).IsRequired();
        builder.Property(t => t.AddressLine2).HasColumnName("ADDRESS_LINE2").HasMaxLength(200);
        builder.Property(t => t.AddressLine3).HasColumnName("ADDRESS_LINE3").HasMaxLength(200);
        builder.Property(t => t.City).HasColumnName("CITY").HasMaxLength(50);
        builder.Property(t => t.State).HasColumnName("STATE").HasMaxLength(50);
        builder.Property(t => t.PinCode).HasColumnName("PIN_CODE").HasMaxLength(10);
        builder.Property(t => t.Country).HasColumnName("COUNTRY").HasMaxLength(50);
        builder.Property(t => t.PhoneNo).HasColumnName("PHONE_NO").HasMaxLength(20);
        builder.Property(t => t.FaxNo).HasColumnName("FAX_NO").HasMaxLength(20);
        builder.Property(t => t.Email).HasColumnName("EMAIL").HasMaxLength(100);
        builder.Property(t => t.TrustStatus).HasColumnName("TRUST_STATUS").HasMaxLength(1).IsFixedLength().HasDefaultValue("A");
        builder.Property(t => t.CreatedDate).HasColumnName("CREATED_DATE").HasPrecision(3).IsRequired();
        builder.Property(t => t.UpdatedDate).HasColumnName("UPDATED_DATE").HasPrecision(3);
        builder.Property(t => t.RegistrarName).HasColumnName("REGISTRAR_NAME").HasMaxLength(65);
        builder.Property(t => t.RegistrarPhone).HasColumnName("REGISTRAR_PHONE").HasMaxLength(20);

        builder.HasIndex(t => t.TrustStatus).HasDatabaseName("IDX_TRUST_MASTER_STATUS");

        builder.HasMany(t => t.FundTypes).WithOne(f => f.Trust).HasForeignKey(f => f.FundTrustCode);
        builder.HasMany(t => t.Roles).WithOne(r => r.Trust).HasForeignKey(r => r.TrTrustCode);
        builder.HasMany(t => t.Approvers).WithOne(a => a.Trust).HasForeignKey(a => a.TrustCode);
        builder.HasMany(t => t.Configurations).WithOne(c => c.Trust).HasForeignKey(c => c.TrustCode);
        builder.HasMany(t => t.AuditLogs).WithOne(a => a.Trust).HasForeignKey(a => a.TrustCode);
        builder.HasMany(t => t.Units).WithOne(u => u.Trust).HasForeignKey(u => u.TrustCode);

        builder.Ignore(t => t.DomainEvents);
    }
}
