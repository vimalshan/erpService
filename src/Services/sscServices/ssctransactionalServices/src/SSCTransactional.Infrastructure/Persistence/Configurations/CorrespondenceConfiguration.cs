using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSCTransactional.Domain.Aggregates;

namespace SSCTransactional.Infrastructure.Persistence.Configurations;

public class CorrespondenceConfiguration : IEntityTypeConfiguration<CorrespondenceAggregate>
{
    public void Configure(EntityTypeBuilder<CorrespondenceAggregate> builder)
    {
        builder.ToTable("DOC_CORRESPOND");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CORR_ID").ValueGeneratedNever();
        builder.Property(x => x.DocId).HasColumnName("CORR_DOCID").IsRequired();
        builder.Property(x => x.AllocationId).HasColumnName("CORR_ALLID").IsRequired();
        builder.Property(x => x.HoldCategory).HasColumnName("CORR_HOLDCAT").IsRequired();
        builder.Property(x => x.HoldType).HasColumnName("CORR_HOLDTYPE").IsRequired();
        builder.Property(x => x.HoldDate).HasColumnName("CORR_HOLDDATE").IsRequired();
        builder.Property(x => x.HoldRemarks).HasColumnName("CORR_HOLDREMARKS").HasMaxLength(200).IsRequired();
        builder.Property(x => x.HoldBy).HasColumnName("CORR_HOLDBY").IsRequired();
        builder.Property(x => x.HoldStatus).HasColumnName("CORR_HOLDSTATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.ReleaseDate).HasColumnName("CORR_RELDATE");
        builder.Property(x => x.ReleaseRemarks).HasColumnName("CORR_RELREMARKS").HasMaxLength(200);
        builder.Property(x => x.ReleasedBy).HasColumnName("CORR_RELBY");
        builder.Property(x => x.HoldNature).HasColumnName("CORR_HOLDNATURE").HasColumnType("decimal(38,0)");

        builder.Ignore(x => x.DomainEvents);

        builder.HasMany(x => x.Attachments)
            .WithOne()
            .HasForeignKey(a => a.CorrespondenceId);
    }
}

public class CorrespondenceAttachmentConfiguration : IEntityTypeConfiguration<CorrespondenceAttachment>
{
    public void Configure(EntityTypeBuilder<CorrespondenceAttachment> builder)
    {
        builder.ToTable("DOC_CORRESPONDATT");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ATT_ID").ValueGeneratedNever();
        builder.Property(x => x.CorrespondenceId).HasColumnName("ATT_CORRID").IsRequired();
        builder.Property(x => x.CorrespondenceStatus).HasColumnName("ATT_CORRSTATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.FilePath).HasColumnName("ATT_FILEPATH").HasMaxLength(200).IsRequired();

        builder.Ignore(x => x.DomainEvents);
    }
}

public class DefectiveAttachmentConfiguration : IEntityTypeConfiguration<DefectiveAttachment>
{
    public void Configure(EntityTypeBuilder<DefectiveAttachment> builder)
    {
        builder.ToTable("DOC_DEFECTIVEATT");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("DEFATT_ID").ValueGeneratedNever();
        builder.Property(x => x.AllocationId).HasColumnName("DEFATT_ALLID").IsRequired();
        builder.Property(x => x.FilePath).HasColumnName("DEFATT_FILEPATH").HasMaxLength(200).IsRequired();

        builder.Ignore(x => x.DomainEvents);
    }
}
