using FilingAndArchiveService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilingAndArchiveService.Infrastructure.Persistence.Configurations;

public class FilingCounterConfiguration : IEntityTypeConfiguration<FilingCounter>
{
    public void Configure(EntityTypeBuilder<FilingCounter> builder)
    {
        builder.ToTable("FILING_COUNTER");
        builder.HasKey(x => x.FilingBuId);
        builder.Property(x => x.FilingBuId).HasColumnName("FILING_BUID").HasMaxLength(25).IsRequired();
        builder.Property(x => x.FileCount).HasColumnName("FILE_COUNT").IsRequired();
    }
}
