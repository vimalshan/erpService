using FilingAndArchiveService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilingAndArchiveService.Infrastructure.Persistence.Configurations;

public class FilingDocPrintConfiguration : IEntityTypeConfiguration<FilingDocPrint>
{
    public void Configure(EntityTypeBuilder<FilingDocPrint> builder)
    {
        builder.ToTable("FILING_DOC_PRINT");
        builder.HasKey(x => x.DocSeq);
        builder.Property(x => x.DocSeq).HasColumnName("DOC_SEQ").ValueGeneratedNever();
        builder.Property(x => x.DocKey).HasColumnName("DOC_KEY").HasMaxLength(50).IsRequired();
        builder.Property(x => x.DocFileNo).HasColumnName("DOC_FILENO").IsRequired();
    }
}
