using IntegrationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationService.Infrastructure.Persistence.Configurations;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("ORA_POMAST");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("PO_SEQID").ValueGeneratedNever();
        builder.Property(e => e.OracleOrgId).HasColumnName("PO_OUID").IsRequired();
        builder.Property(e => e.OraclePoId).HasColumnName("PO_ID").IsRequired();
        builder.Property(e => e.PoNumber).HasColumnName("PO_NO").HasMaxLength(25).IsRequired();
        builder.Property(e => e.VendorSiteId).HasColumnName("PO_VENDORSITEID").IsRequired();

        builder.OwnsOne(e => e.PaymentTerms, pt =>
        {
            pt.Property(p => p.DueDays).HasColumnName("PO_DUEDAYS").IsRequired();
            pt.Property(p => p.DueDayMonthOffset).HasColumnName("PO_DUE_DAY_MONTHOFF").IsRequired();
            pt.Property(p => p.MonthForward).HasColumnName("PO_MONTHFORWARD").IsRequired();
        });

        builder.HasIndex(e => e.OraclePoId).IsUnique();

        builder.HasMany(e => e.MaterialReceipts)
            .WithOne()
            .HasForeignKey(e => e.PurchaseOrderId)
            .HasPrincipalKey(e => e.Id);

        builder.Metadata.FindNavigation(nameof(PurchaseOrder.MaterialReceipts))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}

public class MaterialReceiptConfiguration : IEntityTypeConfiguration<MaterialReceiptCertificate>
{
    public void Configure(EntityTypeBuilder<MaterialReceiptCertificate> builder)
    {
        builder.ToTable("ORA_MRCMAST");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("MRC_SEQID").ValueGeneratedNever();
        builder.Property(e => e.PurchaseOrderId).HasColumnName("MRC_POID").IsRequired();
        builder.Property(e => e.MrcNumber).HasColumnName("MRC_NO").HasMaxLength(25).IsRequired();
        builder.Property(e => e.SequenceNumber).HasColumnName("MRC_SEQNO");
        builder.Property(e => e.ReceiveDate).HasColumnName("MRC_RECDATE").HasPrecision(3);
        builder.Property(e => e.VendorId).HasColumnName("MRC_VENDID");
        builder.Property(e => e.VendorSiteId).HasColumnName("MRC_VENSITEID");
    }
}

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable("ORA_VENDORMAST");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("VENDOR_ID").ValueGeneratedNever();
        builder.Property(e => e.VendorName).HasColumnName("VENDOR_NAME").HasMaxLength(200).IsRequired();
        builder.Property(e => e.VendorCode).HasColumnName("VENDOR_CODE").HasMaxLength(200).IsRequired();

        builder.HasMany(e => e.VendorSites)
            .WithOne()
            .HasForeignKey(e => e.VendorId)
            .HasPrincipalKey(e => e.Id);

        builder.Metadata.FindNavigation(nameof(Vendor.VendorSites))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}

public class VendorSiteConfiguration : IEntityTypeConfiguration<VendorSite>
{
    public void Configure(EntityTypeBuilder<VendorSite> builder)
    {
        builder.ToTable("ORA_VENDORSITEMAST");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("VENDOR_SITEID").ValueGeneratedNever();
        builder.Property(e => e.VendorId).HasColumnName("VENDOR_ID").IsRequired();
        builder.Property(e => e.SiteCode).HasColumnName("VENDOR_SITECODE").HasMaxLength(200).IsRequired();
        builder.Property(e => e.OracleOuId).HasColumnName("VENDOR_OUID").HasMaxLength(25).IsRequired();

        builder.HasMany(e => e.BuMappings)
            .WithOne()
            .HasForeignKey(e => e.VendorSiteId)
            .HasPrincipalKey(e => e.Id);

        builder.Metadata.FindNavigation(nameof(VendorSite.BuMappings))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}

public class VendorSiteBuMappingConfiguration : IEntityTypeConfiguration<VendorSiteBuMapping>
{
    public void Configure(EntityTypeBuilder<VendorSiteBuMapping> builder)
    {
        builder.ToTable("ORA_VENDORSITEBUMAP");
        builder.HasKey(e => e.VendorSiteId);
        builder.Property(e => e.VendorSiteId).HasColumnName("VENDOR_SITEID").ValueGeneratedNever();
        builder.Property(e => e.BuId).HasColumnName("VENDOR_BUID").IsRequired();
    }
}

public class OrganizationUnitConfiguration : IEntityTypeConfiguration<OrganizationUnit>
{
    public void Configure(EntityTypeBuilder<OrganizationUnit> builder)
    {
        builder.ToTable("ORA_OUMAST");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("OU_ID").HasMaxLength(25);
        builder.Property(e => e.OuName).HasColumnName("OU_NAME").HasMaxLength(250).IsRequired();
        builder.Property(e => e.BuId).HasColumnName("OU_BUID").HasMaxLength(25).IsRequired();
    }
}

public class OuBuMappingConfiguration : IEntityTypeConfiguration<OuBuMapping>
{
    public void Configure(EntityTypeBuilder<OuBuMapping> builder)
    {
        builder.ToTable("ORA_OU_BUMAP");
        builder.HasNoKey();
        builder.Property(e => e.OuId).HasColumnName("OU_ID").IsRequired();
        builder.Property(e => e.BuId).HasColumnName("OU_BUID").HasMaxLength(25).IsRequired();
    }
}
