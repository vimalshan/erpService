using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSCTransactional.Domain.Entities;

namespace SSCTransactional.Infrastructure.Persistence.Configurations;

public class DocumentApprovalConfiguration : IEntityTypeConfiguration<DocumentApproval>
{
    public void Configure(EntityTypeBuilder<DocumentApproval> builder)
    {
        builder.ToTable("DOC_APPDET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("APP_SEQID").ValueGeneratedNever();
        builder.Property(x => x.DocId).HasColumnName("APP_DOCID").IsRequired();
        builder.Property(x => x.ApproverUserId).HasColumnName("APP_USERID").IsRequired();
        builder.Property(x => x.Status).HasColumnName("APP_STATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.Remarks).HasColumnName("APP_REMARKS").HasMaxLength(200);
        builder.Property(x => x.ApprovalDate).HasColumnName("APP_DATE").IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}

public class RescanDetailConfiguration : IEntityTypeConfiguration<RescanDetail>
{
    public void Configure(EntityTypeBuilder<RescanDetail> builder)
    {
        builder.ToTable("DOC_RESCANDET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("RESCAN_ID").ValueGeneratedNever();
        builder.Property(x => x.DocId).HasColumnName("RESCAN_DOCID").IsRequired();
        builder.Property(x => x.AllocationId).HasColumnName("RESCAN_ALLID").IsRequired();
        builder.Property(x => x.Status).HasColumnName("RESCAN_STATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.RescanDate).HasColumnName("RESCAN_DATE").IsRequired();
        builder.Property(x => x.RescanTo).HasColumnName("RESCAN_TO").HasMaxLength(1).IsRequired();
        builder.Property(x => x.RescanRemarks).HasColumnName("RESCAN_REMARKS").HasMaxLength(100).IsRequired();
        builder.Property(x => x.CompletedOn).HasColumnName("RESCAN_ON");
        builder.Property(x => x.CompletedBy).HasColumnName("RESCAN_BY");
        builder.Property(x => x.CompletionRemarks).HasColumnName("RESCAN_COMPLETIONREM").HasMaxLength(100);
        builder.Property(x => x.FilePath).HasColumnName("RESCAN_FILEPATH").HasMaxLength(200);
        builder.Ignore(x => x.DomainEvents);
    }
}

public class RevokeDetailConfiguration : IEntityTypeConfiguration<RevokeDetail>
{
    public void Configure(EntityTypeBuilder<RevokeDetail> builder)
    {
        builder.ToTable("DOC_REVOKEDET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("DOC_REVOKEDETID").ValueGeneratedNever();
        builder.Property(x => x.DocId).HasColumnName("DOC_ID").IsRequired();
        builder.Property(x => x.RevokeRemarks).HasColumnName("DOC_REVOKEREMARKS").HasMaxLength(1000).IsRequired();
        builder.Property(x => x.RevokeStatus).HasColumnName("DOC_REVOKESTATUS").HasMaxLength(10).IsRequired();
        builder.Property(x => x.RevokedBy).HasColumnName("DOC_REVOKEDBY").IsRequired();
        builder.Property(x => x.RevokedOn).HasColumnName("DOC_REVOKEDON").IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}

public class DocumentApproverConfiguration : IEntityTypeConfiguration<DocumentApprover>
{
    public void Configure(EntityTypeBuilder<DocumentApprover> builder)
    {
        builder.ToTable("DOC_APPROVER");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("DOC_APPRID").ValueGeneratedNever();
        builder.Property(x => x.BusinessUnit).HasColumnName("DOC_BU").HasMaxLength(25).IsRequired();
        builder.Property(x => x.LocationId).HasColumnName("DOC_LOC").IsRequired();
        builder.Property(x => x.ApproverType).HasColumnName("DOC_APPRTYPE").HasMaxLength(1).IsRequired();
        builder.Property(x => x.ApproverEmpId).HasColumnName("DOC_APPREMPID").IsRequired();
        builder.Property(x => x.EnteredBy).HasColumnName("DOC_ENTBY").IsRequired();
        builder.Property(x => x.EnteredOn).HasColumnName("DOC_ENTON").IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}
