using FilingAndArchiveService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilingAndArchiveService.Infrastructure.Persistence.Configurations;

public class FileMasterConfiguration : IEntityTypeConfiguration<FileMaster>
{
    public void Configure(EntityTypeBuilder<FileMaster> builder)
    {
        builder.ToTable("FILE_MASTER");

        builder.HasKey(x => x.FileId);
        builder.Property(x => x.FileId).HasColumnName("FILE_ID").ValueGeneratedNever();
        builder.Property(x => x.FileOrgId).HasColumnName("FILE_ORGID").HasMaxLength(25).IsRequired();
        builder.Property(x => x.FileYear).HasColumnName("FILE_YEAR").IsRequired();
        builder.Property(x => x.FileNo).HasColumnName("FILE_NO").HasMaxLength(25).IsRequired();
        builder.Property(x => x.FileStatus).HasColumnName("FILE_STATUS").HasMaxLength(1).IsFixedLength().IsRequired();
        builder.Property(x => x.FileRemarks).HasColumnName("FILE_REMARKS").HasMaxLength(200);
        builder.Property(x => x.FilePodNo).HasColumnName("FILE_PODNO").HasMaxLength(50);
        builder.Property(x => x.FileCourierName).HasColumnName("FILE_COURIERNAME").HasMaxLength(200);
        builder.Property(x => x.FileCreatedOn).HasColumnName("FILE_CREATEDON").HasPrecision(3).IsRequired();
        builder.Property(x => x.FileCreatedBy).HasColumnName("FILE_CREATEDBY").IsRequired();
        builder.Property(x => x.FileUpdatedOn).HasColumnName("FILE_UPDATEDON").HasPrecision(3).IsRequired();
        builder.Property(x => x.FileUpdatedBy).HasColumnName("FILE_UPDATEDBY").IsRequired();
        builder.Property(x => x.FileDispatchedOn).HasColumnName("FILE_DISPATCHEDON").HasPrecision(3);
        builder.Property(x => x.FileDispatchedBy).HasColumnName("FILE_DISPATCHEDBY");

        builder.Ignore(x => x.DomainEvents);

        builder.HasIndex(x => new { x.FileOrgId, x.FileNo }).HasDatabaseName("IX_FILE_MASTER_ORG_FILENO");
        builder.HasIndex(x => x.FileYear).HasDatabaseName("IX_FILE_MASTER_YEAR");
        builder.HasIndex(x => x.FileStatus).HasDatabaseName("IX_FILE_MASTER_STATUS");
    }
}
