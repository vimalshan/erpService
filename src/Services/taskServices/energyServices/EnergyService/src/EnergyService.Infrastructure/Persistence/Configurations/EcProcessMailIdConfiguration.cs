using EnergyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnergyService.Infrastructure.Persistence.Configurations;

public class EcProcessMailIdConfiguration : IEntityTypeConfiguration<EcProcessMailId>
{
    public void Configure(EntityTypeBuilder<EcProcessMailId> builder)
    {
        builder.ToTable("EC_PROCESS_MAILID");
        builder.HasKey(e => e.PmId);

        builder.Property(e => e.PmId).HasColumnName("PM_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.PmProcessId).HasColumnName("PM_PROCESS_ID").IsRequired();
        builder.Property(e => e.PmMailId).HasColumnName("PM_MAIL_ID").HasMaxLength(65).IsRequired();
        builder.Property(e => e.PmDeliveryType).HasColumnName("PM_DELIVERY_TYPE").HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(e => e.PmStartDate).HasColumnName("PM_START_DATE").HasPrecision(3).IsRequired();
        builder.Property(e => e.PmCloseDate).HasColumnName("PM_CLOSE_DATE").HasPrecision(3);
        builder.Property(e => e.PmLastModifiedBy).HasColumnName("PM_LAST_MODIFIEDBY").IsRequired();
        builder.Property(e => e.PmLastModifiedOn).HasColumnName("PM_LAST_MODIFIEDON").HasMaxLength(20).IsRequired();

        builder.Ignore(e => e.DomainEvents);
    }
}
