using HRDocumentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRDocumentService.Infrastructure.Persistence.Configurations;

public class HRDocumentConfiguration : IEntityTypeConfiguration<HRDocument>
{
    public void Configure(EntityTypeBuilder<HRDocument> builder)
    {
        builder.ToTable("HRDOC_DET");
        builder.HasKey(e => e.DocId);

        builder.Property(e => e.DocId).HasColumnName("DOC_ID").ValueGeneratedNever();
        builder.Property(e => e.DocNo).HasColumnName("DOC_NO");
        builder.Property(e => e.DocType).HasColumnName("DOC_TYPE").HasMaxLength(3).IsRequired();
        builder.Property(e => e.DocPayRefNo).HasColumnName("DOC_PAYREFNO");
        builder.Property(e => e.DocLocId).HasColumnName("DOC_LOCID");
        builder.Property(e => e.DocUnitId).HasColumnName("DOC_UNITID");
        builder.Property(e => e.DocRemarks).HasColumnName("DOC_REMARKS").HasMaxLength(100).IsRequired();
        builder.Property(e => e.DocUserId).HasColumnName("DOC_USERID");
        builder.Property(e => e.DocRefNo).HasColumnName("DOC_REFNO").HasMaxLength(50);
        builder.Property(e => e.DocRefName).HasColumnName("DOC_REFNAME").HasMaxLength(200);
        builder.Property(e => e.DocCreatedOn).HasColumnName("DOC_CREATEDON").HasPrecision(3);
        builder.Property(e => e.DocDocStatus).HasColumnName("DOC_DOCSTATUS").HasMaxLength(2).IsRequired();
        builder.Property(e => e.DocSource).HasColumnName("DOC_SOURCE").HasMaxLength(3).IsRequired();
        builder.Property(e => e.DocActionStatus).HasColumnName("DOC_ACTIONSTATUS").HasMaxLength(1);
        builder.Property(e => e.DocActionTakenOn).HasColumnName("DOC_ACTIONTAKENON").HasPrecision(3);
        builder.Property(e => e.DocActionTakenBy).HasColumnName("DOC_ACTIONTAKENBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.DocFilePath).HasColumnName("DOC_FILEPATH").HasMaxLength(200);
        builder.Property(e => e.DocCancelFlag).HasColumnName("DOC_CANCELFLAG").HasMaxLength(1);
        builder.Property(e => e.DocCancelBy).HasColumnName("DOC_CANCELBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.DocCancelOn).HasColumnName("DOC_CANCELON").HasPrecision(3);
        builder.Property(e => e.DocPayBy).HasColumnName("DOC_PAYBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.DocRejectRemarks).HasColumnName("DOC_REJECTREMARKS").HasMaxLength(200);

        builder.Ignore(e => e.Version);
        builder.Ignore(e => e.DomainEvents);

        builder.HasMany(e => e.Files)
            .WithOne()
            .HasForeignKey(f => f.FileDocId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Receipts)
            .WithOne()
            .HasForeignKey(r => r.HRRecHRDocId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class HRDocumentFileConfiguration : IEntityTypeConfiguration<HRDocumentFile>
{
    public void Configure(EntityTypeBuilder<HRDocumentFile> builder)
    {
        builder.ToTable("HRDOC_SSFILELIST");
        builder.HasKey(e => e.FileId);

        builder.Property(e => e.FileId).HasColumnName("FILE_ID").ValueGeneratedNever();
        builder.Property(e => e.FileDocId).HasColumnName("FILE_DOCID");
        builder.Property(e => e.FilePath).HasColumnName("FILE_PATH").HasMaxLength(25).IsRequired();
        builder.Property(e => e.FileName).HasColumnName("FILE_NAME").HasMaxLength(200).IsRequired();

        builder.Ignore(e => e.DomainEvents);
    }
}

public class HRDocumentReceiptConfiguration : IEntityTypeConfiguration<HRDocumentReceipt>
{
    public void Configure(EntityTypeBuilder<HRDocumentReceipt> builder)
    {
        builder.ToTable("HRDOC_RECDET");
        builder.HasKey(e => e.HRRecId);

        builder.Property(e => e.HRRecId).HasColumnName("HRREC_ID").ValueGeneratedNever();
        builder.Property(e => e.HRRecEnvId).HasColumnName("HRREC_ENVID");
        builder.Property(e => e.HRRecHRDocId).HasColumnName("HRREC_HRDOCID");
        builder.Property(e => e.HRRecUpdatedBy).HasColumnName("HRREC_UPDATEDBY");
        builder.Property(e => e.HRRecUpdatedOn).HasColumnName("HRREC_UPDATEDON").HasPrecision(3);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class HRDocumentCounterConfiguration : IEntityTypeConfiguration<HRDocumentCounter>
{
    public void Configure(EntityTypeBuilder<HRDocumentCounter> builder)
    {
        builder.ToTable("HRDOC_COUNTER");
        builder.HasNoKey();

        builder.Property(e => e.DocNo).HasColumnName("DOC_NO");

        builder.Ignore(e => e.DomainEvents);
    }
}
