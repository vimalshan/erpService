using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CardManagement.Domain.Entities;

namespace CardManagement.Infrastructure.Persistence.Configurations;

public class CanteenCardMapConfiguration : IEntityTypeConfiguration<CanteenCardMap>
{
    public void Configure(EntityTypeBuilder<CanteenCardMap> builder)
    {
        builder.ToTable("CANTEEN_CARD_MAP");
        builder.HasNoKey();

        builder.Property(x => x.SysId).HasColumnName("CC_SYSID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.CanteenUnit).HasColumnName("CC_CAN_UNT");
        builder.Property(x => x.CardNumber).HasColumnName("CC_CRD_NUM").HasMaxLength(50).IsRequired();
        builder.Property(x => x.EffectiveDate).HasColumnName("CC_EFF_DAT");
        builder.Property(x => x.ClosingDate).HasColumnName("CC_CLS_DAT");
        builder.Property(x => x.UpdatedByUser).HasColumnName("CC_UPD_USR").HasColumnType("decimal(38,0)");
        builder.Property(x => x.UpdatedDate).HasColumnName("CC_UPD_DAT");

        builder.Ignore(x => x.DomainEvents);
    }
}
