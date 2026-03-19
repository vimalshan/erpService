using FilingAndArchiveService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilingAndArchiveService.Infrastructure.Persistence.Configurations;

public class FilingDocErrorListConfiguration : IEntityTypeConfiguration<FilingDocErrorList>
{
    public void Configure(EntityTypeBuilder<FilingDocErrorList> builder)
    {
        builder.ToTable("FILINGDOC_ERROR_LIST");
        builder.HasNoKey();
        builder.Property(x => x.DocKey).HasColumnName("DOC_KEY").HasMaxLength(50);
        builder.Property(x => x.Remarks).HasColumnName("REMARKS").HasMaxLength(4000);
        builder.Property(x => x.SysId).HasColumnName("SYS_ID");
        builder.Property(x => x.AccountingDate).HasColumnName("ACCOUNTING_DATE").HasPrecision(3);
        builder.Property(x => x.Flag).HasColumnName("FLAG").HasMaxLength(10);
        builder.Property(x => x.Status).HasColumnName("STATUS").HasMaxLength(100);
        builder.Property(x => x.Sno).HasColumnName("SNO");
    }
}
