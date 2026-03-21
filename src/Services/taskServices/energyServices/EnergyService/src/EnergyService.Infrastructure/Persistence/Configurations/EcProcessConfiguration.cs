using EnergyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnergyService.Infrastructure.Persistence.Configurations;

public class EcProcessConfiguration : IEntityTypeConfiguration<EcProcess>
{
    public void Configure(EntityTypeBuilder<EcProcess> builder)
    {
        builder.ToTable("EC_PROCESS");
        builder.HasKey(e => e.EcProcessId);

        builder.Property(e => e.EcProcessId).HasColumnName("EC_PROCESS_ID").ValueGeneratedNever();
        builder.Property(e => e.EcProcessDesc).HasColumnName("EC_PROCESS_DESC").HasMaxLength(65).IsRequired();
        builder.Property(e => e.EcUnitCode).HasColumnName("EC_UNIT_CODE").HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(e => e.EcCloseFlag).HasColumnName("EC_CLOSE_FLAG").HasMaxLength(1).IsFixedLength().IsRequired();
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").IsRequired();
        builder.Property(e => e.LastModifiedOn).HasColumnName("LAST_MODIFIED_ON").HasPrecision(3).IsRequired();

        builder.HasMany(e => e.ProcessAccesses).WithOne(a => a.Process).HasForeignKey(a => a.PaProcessId);
        builder.HasMany(e => e.ProcessMailIds).WithOne(m => m.Process).HasForeignKey(m => m.PmProcessId);
        builder.HasMany(e => e.Readings).WithOne(r => r.Process).HasForeignKey(r => r.EbProcessId);

        builder.Ignore(e => e.DomainEvents);
    }
}
