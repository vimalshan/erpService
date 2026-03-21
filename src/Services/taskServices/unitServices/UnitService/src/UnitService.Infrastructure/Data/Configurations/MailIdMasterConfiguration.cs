using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnitService.Domain.Entities;
using UnitService.Domain.ValueObjects;

namespace UnitService.Infrastructure.Data.Configurations;

public class MailIdMasterConfiguration : IEntityTypeConfiguration<MailIdMaster>
{
    public void Configure(EntityTypeBuilder<MailIdMaster> builder)
    {
        builder.ToTable("UM_MAILID_MASTER");

        builder.HasKey(e => e.MailId);
        builder.Property(e => e.MailId).HasColumnName("MM_ID");
        builder.Property(e => e.UnitCode).HasColumnName("MM_UNIT_CODE").HasMaxLength(3).IsRequired()
            .HasConversion(v => v.Value, v => UnitCode.From(v));
        builder.Property(e => e.EmailAddress).HasColumnName("MM_MAIL_ID").HasMaxLength(65).IsRequired();
        builder.Property(e => e.DeliveryType).HasColumnName("MM_DELIVERY_TYPE").HasMaxLength(3).IsRequired();
        builder.Property(e => e.StartDate).HasColumnName("MM_START_DATE").HasMaxLength(255).IsRequired();
        builder.Property(e => e.CloseDate).HasColumnName("MM_CLOSE_DATE").HasMaxLength(255);
        builder.Property(e => e.LastModifiedBy).HasColumnName("MM_LAST_MODIFIEDBY").IsRequired();
        builder.Property(e => e.LastModifiedOn).HasColumnName("MM_LAST_MODIFIEDON").HasMaxLength(30).IsRequired();
        builder.Property(e => e.Module).HasColumnName("MM_MODULE").HasMaxLength(5).IsRequired();
    }
}
