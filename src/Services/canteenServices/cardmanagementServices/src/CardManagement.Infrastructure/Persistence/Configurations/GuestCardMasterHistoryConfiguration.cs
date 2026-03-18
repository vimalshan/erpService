using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CardManagement.Domain.Entities;

namespace CardManagement.Infrastructure.Persistence.Configurations;

public class GuestCardMasterHistoryConfiguration : IEntityTypeConfiguration<GuestCardMasterHistory>
{
    public void Configure(EntityTypeBuilder<GuestCardMasterHistory> builder)
    {
        builder.ToTable("GUEST_CARD_MASTER_HIS");
        builder.HasNoKey();

        builder.Property(x => x.CanteenUnit).HasColumnName("GC_COM_COD").IsRequired();
        builder.Property(x => x.CardSequence).HasColumnName("GC_CRD_SEQ").IsRequired();
        builder.Property(x => x.CardNumber).HasColumnName("GC_CRD_NUM").HasMaxLength(20);
        builder.Property(x => x.CardName).HasColumnName("GC_CRD_NAM").HasMaxLength(50);
        builder.Property(x => x.ReportingUnit).HasColumnName("GC_REP_UNT").HasMaxLength(3).IsFixedLength();
        builder.Property(x => x.ReportingDepartment).HasColumnName("GC_CRD_DEP").HasColumnType("decimal(38,0)");
        builder.Property(x => x.CardType).HasColumnName("GC_CRD_TYP").HasMaxLength(1).IsFixedLength();
        builder.Property(x => x.ModifiedByUser).HasColumnName("GC_MOD_USR").HasColumnType("decimal(38,0)");
        builder.Property(x => x.ModifiedOn).HasColumnName("GC_MOD_ON");
    }
}
