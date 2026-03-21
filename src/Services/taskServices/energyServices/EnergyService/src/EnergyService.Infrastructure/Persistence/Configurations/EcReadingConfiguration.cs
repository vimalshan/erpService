using EnergyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnergyService.Infrastructure.Persistence.Configurations;

public class EcReadingConfiguration : IEntityTypeConfiguration<EcReading>
{
    public void Configure(EntityTypeBuilder<EcReading> builder)
    {
        builder.ToTable("EC_READING");
        builder.HasKey(e => e.EbId);

        builder.Property(e => e.EbId).HasColumnName("EB_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.EbUnitCode).HasColumnName("EB_UNIT_CODE").HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(e => e.EbProcessId).HasColumnName("EB_PROCESS_ID").IsRequired();
        builder.Property(e => e.EbDate).HasColumnName("EB_DATE").HasPrecision(3).IsRequired();
        builder.Property(e => e.EbTarget).HasColumnName("EB_TARGET");
        builder.Property(e => e.EbReading).HasColumnName("EB_READING");
        builder.Property(e => e.EbResetReading).HasColumnName("EB_RESET_READING");
        builder.Property(e => e.EbActualUsage).HasColumnName("EB_ACTUAL_USAGE");
        builder.Property(e => e.EbToDate).HasColumnName("EB_TODATE");
        builder.Property(e => e.EbRemarks).HasColumnName("EB_REMARKS").HasMaxLength(100);
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").IsRequired();
        builder.Property(e => e.LastModifiedOn).HasColumnName("LAST_MODIFIED_ON").HasPrecision(3).IsRequired();

        builder.Ignore(e => e.DomainEvents);
    }
}
