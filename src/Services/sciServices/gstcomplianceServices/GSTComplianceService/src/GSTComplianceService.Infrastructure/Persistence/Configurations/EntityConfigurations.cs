using GSTComplianceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSTComplianceService.Infrastructure.Persistence.Configurations;

public class GstMainConfiguration : IEntityTypeConfiguration<GstMain>
{
    public void Configure(EntityTypeBuilder<GstMain> builder)
    {
        builder.ToTable("GST_MAIN");
        builder.HasKey(e => e.GstId);
        builder.Property(e => e.GstId).HasColumnName("GST_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.GstType).HasColumnName("GST_TYPE").HasMaxLength(1);
        builder.Property(e => e.GstPanNo).HasColumnName("GST_PANNO").HasMaxLength(20).IsRequired();
        builder.Property(e => e.GstEmailId).HasColumnName("GST_EMAILID").HasMaxLength(200);
        builder.Property(e => e.GstMobileNo).HasColumnName("GST_MOBILENO");
        builder.Property(e => e.GstCreatedOn).HasColumnName("GST_CREATEDON");
        builder.Property(e => e.GstModifiedOn).HasColumnName("GST_MODIFIEDON");
        builder.Property(e => e.GstVendorId).HasColumnName("GST_VENDORID");
        builder.Property(e => e.GstVendorNameFlag).HasColumnName("GST_VENDORNAMEFLAG").HasMaxLength(1);
        builder.Property(e => e.GstVendorName).HasColumnName("GST_VENDORNAME").HasMaxLength(200);
        builder.Property(e => e.GstVendConst).HasColumnName("GST_VENDCONST");
        builder.Property(e => e.GstVendAddFlag).HasColumnName("GST_VENDADDFLAG").HasMaxLength(1);
        builder.Property(e => e.GstVendAddLine1).HasColumnName("GST_VENDADDLINE1").HasMaxLength(200);
        builder.Property(e => e.GstVendAddLine2).HasColumnName("GST_VENDADDLINE2").HasMaxLength(100);
        builder.Property(e => e.GstVendAddLine3).HasColumnName("GST_VENDADDLINE3").HasMaxLength(100);
        builder.Property(e => e.GstVendAddLine4).HasColumnName("GST_VENDADDLINE4").HasMaxLength(100);
        builder.Property(e => e.GstVendCity).HasColumnName("GST_VENDCITY").HasMaxLength(100);
        builder.Property(e => e.GstVendCityName).HasColumnName("GST_VENDCITYNAME").HasMaxLength(100);
        builder.Property(e => e.GstVendState).HasColumnName("GST_VENDSTATE").HasMaxLength(100);
        builder.Property(e => e.GstVendPincode).HasColumnName("GST_VENDPINCODE").HasMaxLength(100);
        builder.Property(e => e.GstRegistrationType).HasColumnName("GST_REGISTRATIONTYPE");
        builder.Property(e => e.GstContactName).HasColumnName("GST_CONTACTNAME").HasMaxLength(100);
        builder.Property(e => e.GstContactEmailId).HasColumnName("GST_CONTACTEMAILID").HasMaxLength(100);
        builder.Property(e => e.GstContactMobileNo).HasColumnName("GST_CONTACTMOBILENO");
        builder.Property(e => e.GstRemarks).HasColumnName("GST_REMARKS").HasMaxLength(200);
        builder.Property(e => e.GstStatus).HasColumnName("GST_STATUS").HasMaxLength(1);
        builder.Property(e => e.GstDigitalFlag).HasColumnName("GST_DIGITALFLAG").HasMaxLength(255).IsRequired();
        builder.Property(e => e.GstGstnCopy).HasColumnName("GST_GSTNCOPY").HasMaxLength(200);
        builder.Property(e => e.GstEnteredByFlag).HasColumnName("GST_ENTEREDBYFLA").HasMaxLength(1);
        builder.Property(e => e.GstEnteredBy).HasColumnName("GST_ENTEREDBY");
        builder.Property(e => e.GstScreenType).HasColumnName("GST_SCREENTYPE").HasMaxLength(1);

        builder.HasMany(e => e.HsnDetails)
            .WithOne(h => h.GstMain)
            .HasForeignKey(h => h.GstHsnGstId);

        builder.HasMany(e => e.ServiceDetails)
            .WithOne(s => s.GstMain)
            .HasForeignKey(s => s.GstSacGstId);

        builder.HasMany(e => e.StateRegDetails)
            .WithOne(r => r.GstMain)
            .HasForeignKey(r => r.GstId);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class GstHsnDetailConfiguration : IEntityTypeConfiguration<GstHsnDetail>
{
    public void Configure(EntityTypeBuilder<GstHsnDetail> builder)
    {
        builder.ToTable("GST_HSNDET");
        builder.HasKey(e => e.GstHsnId);
        builder.Property(e => e.GstHsnId).HasColumnName("GSTHSN_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.GstHsnGstId).HasColumnName("GSTHSN_GSTID").IsRequired();
        builder.Property(e => e.GstHsnProductName).HasColumnName("GSTHSN_PRODUCTNAME").HasMaxLength(100);
        builder.Property(e => e.GstHsnCode).HasColumnName("GSTHSN_HSNCODE").HasMaxLength(50);
        builder.Property(e => e.GstHsnRemarks).HasColumnName("GSTHSN_REMARKS").HasMaxLength(200);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class GstServiceDetailConfiguration : IEntityTypeConfiguration<GstServiceDetail>
{
    public void Configure(EntityTypeBuilder<GstServiceDetail> builder)
    {
        builder.ToTable("GST_SERVDET");
        builder.HasKey(e => e.GstSacId);
        builder.Property(e => e.GstSacId).HasColumnName("GSTSAC_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.GstSacGstId).HasColumnName("GSTSAC_GSTID").IsRequired();
        builder.Property(e => e.GstSacServiceName).HasColumnName("GSTSAC_SERVICENAME").HasMaxLength(100);
        builder.Property(e => e.GstSacCode).HasColumnName("GSTSAC_SACCODE").HasMaxLength(50);
        builder.Property(e => e.GstSacRemarks).HasColumnName("GSTSAC_REMARKS").HasMaxLength(200);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class GstStateRegDetailConfiguration : IEntityTypeConfiguration<GstStateRegDetail>
{
    public void Configure(EntityTypeBuilder<GstStateRegDetail> builder)
    {
        builder.ToTable("GST_STATEREGDET");
        builder.HasKey(e => e.GstTinId);
        builder.Property(e => e.GstTinId).HasColumnName("GST_TINID").ValueGeneratedOnAdd();
        builder.Property(e => e.GstId).HasColumnName("GST_ID").IsRequired();
        builder.Property(e => e.GstState).HasColumnName("GST_STATE").HasMaxLength(20);
        builder.Property(e => e.GstAddress).HasColumnName("GST_ADDRESS").HasMaxLength(200);
        builder.Property(e => e.GstVendCity).HasColumnName("GST_VENDCITY").HasMaxLength(100);
        builder.Property(e => e.GstVendCityName).HasColumnName("GST_VENDCITYNAME").HasMaxLength(100);
        builder.Property(e => e.GstVendPincode).HasColumnName("GST_VENDPINCODE").HasMaxLength(6);
        builder.Property(e => e.GstTinNo).HasColumnName("GST_TINNO").HasMaxLength(50);
        builder.Property(e => e.GstExcNo).HasColumnName("GST_EXCNO").HasMaxLength(50);
        builder.Property(e => e.GstSerNo).HasColumnName("GST_SERNO").HasMaxLength(50);
        builder.Property(e => e.GstGstinNo).HasColumnName("GST_GSTINNO").HasMaxLength(50);
        builder.Property(e => e.GstArnNo).HasColumnName("GST_ARNNO").HasMaxLength(50);
        builder.Property(e => e.GstArnCopy).HasColumnName("GST_ARNCOPY").HasMaxLength(200);
        builder.Property(e => e.GstArnTempFile).HasColumnName("GST_ARNTEMPFILE").HasMaxLength(200);
        builder.Property(e => e.GstContactPerson).HasColumnName("GST_CONTACTPERSON").HasMaxLength(100);
        builder.Property(e => e.GstEmailId).HasColumnName("GST_EMAILID").HasMaxLength(100);
        builder.Property(e => e.GstMobileNo).HasColumnName("GST_MOBILENO").HasMaxLength(10);
        builder.Property(e => e.GstRemarks).HasColumnName("GST_REMARKS").HasMaxLength(200);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class GstSupplierConfiguration : IEntityTypeConfiguration<GstSupplier>
{
    public void Configure(EntityTypeBuilder<GstSupplier> builder)
    {
        builder.ToTable("GST_SUPPLIER");
        builder.HasKey(e => e.SupplierNumber);
        builder.Property(e => e.SupplierNumber).HasColumnName("SUPPLIER_NUMBER");
        builder.Property(e => e.SupplierName).HasColumnName("SUPPLIER_NAME").HasMaxLength(200).IsRequired();
        builder.Property(e => e.EmailAddress).HasColumnName("EMAIL_ADDRESS").HasMaxLength(50);
        builder.Property(e => e.OperatingUnit).HasColumnName("OU").HasMaxLength(200);
        builder.Property(e => e.PanNo).HasColumnName("PAN_NO");
        builder.Ignore(e => e.DomainEvents);
    }
}
