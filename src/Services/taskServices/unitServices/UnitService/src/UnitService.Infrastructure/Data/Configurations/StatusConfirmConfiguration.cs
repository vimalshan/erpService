using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnitService.Domain.Entities;
using UnitService.Domain.ValueObjects;

namespace UnitService.Infrastructure.Data.Configurations;

public class StatusConfirmConfiguration : IEntityTypeConfiguration<StatusConfirm>
{
    public void Configure(EntityTypeBuilder<StatusConfirm> builder)
    {
        builder.ToTable("UM_STATUS_CONFIRM");

        builder.HasNoKey();
        builder.Property(e => e.UnitCode).HasColumnName("STATUS_UNIT_CODE").HasMaxLength(3).IsRequired()
            .HasConversion(v => v.Value, v => UnitCode.From(v));
        builder.Property(e => e.StatusDate).HasColumnName("STATUS_DATE").HasPrecision(3).IsRequired();
        builder.Property(e => e.ConfirmedBy).HasColumnName("STATUS_CONFIRM_BY").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.ConfirmedOn).HasColumnName("STATUS_CONFIRM_ON").HasPrecision(3).IsRequired();
    }
}
