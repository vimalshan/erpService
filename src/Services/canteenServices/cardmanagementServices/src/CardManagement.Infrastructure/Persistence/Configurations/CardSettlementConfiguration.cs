using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CardManagement.Domain.Entities;

namespace CardManagement.Infrastructure.Persistence.Configurations;

public class CardSettlementConfiguration : IEntityTypeConfiguration<CardSettlement>
{
    public void Configure(EntityTypeBuilder<CardSettlement> builder)
    {
        builder.ToTable("CARD_SETTLEMENT");
        builder.HasNoKey();

        builder.Property(x => x.SysId).HasColumnName("ST_SYSID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.CanteenUnit).HasColumnName("ST_CAN_UNT");
        builder.Property(x => x.CardNumber).HasColumnName("ST_CRD_NUM").HasMaxLength(50);
        builder.Property(x => x.SettlementDate).HasColumnName("ST_SET_DAT");
        builder.Property(x => x.UpdatedByUser).HasColumnName("ST_UPD_USR").HasColumnType("decimal(38,0)");
        builder.Property(x => x.UpdatedDate).HasColumnName("ST_UPD_DAT");

        builder.Ignore(x => x.DomainEvents);
    }
}
