using CategoryAndVendorService.Domain.Entities;
using CategoryAndVendorService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CategoryAndVendorService.Infrastructure.Persistence.Configurations;

public class MainCategoryConfiguration : IEntityTypeConfiguration<MainCategory>
{
    public void Configure(EntityTypeBuilder<MainCategory> builder)
    {
        builder.ToTable("MAINCAT_MAST");
        builder.HasKey(e => e.MainCatId);
        builder.Property(e => e.MainCatId).HasColumnName("MAINCAT_ID").ValueGeneratedNever();
        builder.Property(e => e.MainCatName).HasColumnName("MAINCAT_NAME").HasMaxLength(200).IsRequired();
        builder.Property(e => e.MainCatPriority).HasColumnName("MAINCAT_PRIORITY").IsRequired();
        builder.Property(e => e.ModifiedBy).HasColumnName("MAINCAT_MODIFIEDBY").IsRequired();
        builder.Property(e => e.ModifiedOn).HasColumnName("MAINCAT_MODIFIEDON").HasPrecision(3).IsRequired();
        builder.Property(e => e.DefaultSubCatId).HasColumnName("MAINCAT_DEFSUBCATID");
        builder.Property(e => e.AvgResponseTime).HasColumnName("MAINCAT_AVGRESTIME");

        builder.HasMany(e => e.SubCategories)
            .WithOne(e => e.MainCategory)
            .HasForeignKey(e => e.MainCatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.ToTable("SUBCAT_MAST");
        builder.HasKey(e => e.SubCatId);
        builder.Property(e => e.SubCatId).HasColumnName("SUBCAT_ID").ValueGeneratedNever();
        builder.Property(e => e.MainCatId).HasColumnName("SUBCAT_MAINID").IsRequired();
        builder.Property(e => e.SubCatName).HasColumnName("SUBCAT_NAME").HasMaxLength(200).IsRequired();
        builder.Property(e => e.ModifiedBy).HasColumnName("SUBCAT_MODIFIEDBY").IsRequired();
        builder.Property(e => e.ModifiedOn).HasColumnName("SUBCAT_MODIFIEDON").HasPrecision(3).IsRequired();

        builder.Ignore(e => e.DomainEvents);
    }
}

public class VendorDocumentConfiguration : IEntityTypeConfiguration<VendorDocument>
{
    public void Configure(EntityTypeBuilder<VendorDocument> builder)
    {
        builder.ToTable("VENDOR_DOCDET");
        builder.HasKey(e => e.VndDocId);
        builder.Property(e => e.VndDocId).HasColumnName("VNDDOC_ID").ValueGeneratedNever();
        builder.Property(e => e.VendorId).HasColumnName("VNDDOC_VENDORID").IsRequired();
        builder.Property(e => e.SiteId).HasColumnName("VNDDOC_SITEID").IsRequired();
        builder.Property(e => e.BuId).HasColumnName("VNDDOC_BUID").IsRequired();
        builder.Property(e => e.InformationCategory).HasColumnName("VNDDOC_INFCAT").IsRequired();
        builder.Property(e => e.Remarks).HasColumnName("VNDDOC_REMARKS").HasMaxLength(2000).IsRequired();
        builder.Property(e => e.DocFlag).HasColumnName("VNDDOC_DOCFLAG").HasMaxLength(1).IsRequired();
        builder.Property(e => e.DocType).HasColumnName("VNDDOC_DOCTYPE");
        builder.Property(e => e.DocRefNo).HasColumnName("VNDDOC_DOCREFNO").HasMaxLength(100);
        builder.Property(e => e.ValidFrom).HasColumnName("VNDDOC_VALIDFROM").HasPrecision(3).IsRequired();
        builder.Property(e => e.ValidTo).HasColumnName("VNDDOC_VALIDTO").HasPrecision(3);
        builder.Property(e => e.ActiveStatus).HasColumnName("VNDDOC_ACTIVESTATUS").HasMaxLength(1).IsRequired();
        builder.Property(e => e.ModifiedBy).HasColumnName("VNDDOC_MODIFIEDBY").IsRequired();
        builder.Property(e => e.ModifiedOn).HasColumnName("VNDDOC_MODIFIEDON").HasPrecision(3).IsRequired();
        builder.Property(e => e.ApprovalRemarks).HasColumnName("VNDDOC_APPREMARKS").HasMaxLength(500);
        builder.Property(e => e.ApprovedBy).HasColumnName("VNDDOC_APPROVEDBY");
        builder.Property(e => e.ApprovedOn).HasColumnName("VNDDOC_APPROVEDON").HasPrecision(3);

        builder.Property(e => e.ApprovalStatus)
            .HasColumnName("VNDDOC_APPSTATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(
                v => v.Code,
                v => ApprovalStatus.FromCode(v));

        builder.HasMany(e => e.Files)
            .WithOne(e => e.VendorDocument)
            .HasForeignKey(e => e.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class VendorDocumentFileConfiguration : IEntityTypeConfiguration<VendorDocumentFile>
{
    public void Configure(EntityTypeBuilder<VendorDocumentFile> builder)
    {
        builder.ToTable("VENDOR_DOCFILE");
        builder.HasKey(e => e.FileId);
        builder.Property(e => e.FileId).HasColumnName("VNDFILE_ID").ValueGeneratedNever();
        builder.Property(e => e.DocumentId).HasColumnName("VNDFILE_DOCID").IsRequired();
        builder.Property(e => e.FileName).HasColumnName("VNDFILE_NAME").HasMaxLength(100).IsRequired();
        builder.Property(e => e.FilePath).HasColumnName("VNDFILE_PATH").HasMaxLength(100);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class SupportDocumentConfiguration : IEntityTypeConfiguration<SupportDocument>
{
    public void Configure(EntityTypeBuilder<SupportDocument> builder)
    {
        builder.ToTable("SUPDOC_DET");
        builder.HasKey(e => e.DocId);
        builder.Property(e => e.DocId).HasColumnName("SUP_DOCID").ValueGeneratedNever();
        builder.Property(e => e.DocCategory).HasColumnName("SUP_DOCCAT").IsRequired();
        builder.Property(e => e.InvoiceDocId).HasColumnName("SUP_INVDOCID").IsRequired();
        builder.Property(e => e.DocKey).HasColumnName("SUP_DOCKEY").HasMaxLength(50);
        builder.Property(e => e.DocStatus).HasColumnName("SUP_DOCSTATUS").HasMaxLength(2).IsRequired();
        builder.Property(e => e.PbgNo).HasColumnName("SUP_PBGNO").HasMaxLength(50);
        builder.Property(e => e.PbgStart).HasColumnName("SUP_PBGSTART").HasPrecision(3);
        builder.Property(e => e.PbgExpDate).HasColumnName("SUP_PBGEXPDATE").HasPrecision(3);
        builder.Property(e => e.Amount).HasColumnName("SUP_AMOUNT");
        builder.Property(e => e.RecDue).HasColumnName("SUP_RECDUE");

        builder.HasMany(e => e.Attachments)
            .WithOne(e => e.SupportDocument)
            .HasForeignKey(e => e.DocId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class SupportDocumentAttachmentConfiguration : IEntityTypeConfiguration<SupportDocumentAttachment>
{
    public void Configure(EntityTypeBuilder<SupportDocumentAttachment> builder)
    {
        builder.ToTable("SUPDOC_ATT");
        builder.HasKey(e => e.AttachmentId);
        builder.Property(e => e.AttachmentId).HasColumnName("SUPDOC_ATTID").ValueGeneratedNever();
        builder.Property(e => e.DocId).HasColumnName("SUPDOC_DOCID").IsRequired();
        builder.Property(e => e.InvoiceDocId).HasColumnName("SUPDOC_INVDOCID").IsRequired();
        builder.Property(e => e.RefFlag).HasColumnName("SUPDOC_REFFLAG").HasMaxLength(1).IsRequired();

        builder.Ignore(e => e.DomainEvents);
    }
}

public class SupportDocumentCounterConfiguration : IEntityTypeConfiguration<SupportDocumentCounter>
{
    public void Configure(EntityTypeBuilder<SupportDocumentCounter> builder)
    {
        builder.ToTable("SUPDOC_COUNTER");
        builder.HasNoKey();
        builder.Property(e => e.BuId).HasColumnName("SUPDOC_BUID").HasMaxLength(25).IsRequired();
        builder.Property(e => e.CounterNo).HasColumnName("SUPDOC_NO").IsRequired();

        builder.Ignore(e => e.DomainEvents);
    }
}
