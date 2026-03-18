using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CardManagement.Domain.Entities;

namespace CardManagement.Infrastructure.Persistence.Configurations;

public class GuestCardMasterConfiguration : IEntityTypeConfiguration<GuestCardMaster>
{
    public void Configure(EntityTypeBuilder<GuestCardMaster> builder)
    {
        builder.ToTable("GUEST_CARD_MASTER");
        builder.HasKey(x => x.CanteenUnit);

        builder.Property(x => x.CanteenUnit).HasColumnName("GC_COM_COD").IsRequired().ValueGeneratedNever();
        builder.Property(x => x.CardSequence).HasColumnName("GC_CRD_SEQ").IsRequired();
        builder.Property(x => x.CardNumber).HasColumnName("GC_CRD_NUM").HasMaxLength(20);
        builder.Property(x => x.CardName).HasColumnName("GC_CRD_NAM").HasMaxLength(50);
        builder.Property(x => x.ReportingUnit).HasColumnName("GC_REP_UNT").HasMaxLength(3).IsFixedLength();
        builder.Property(x => x.ReportingDepartment).HasColumnName("GC_CRD_DEP").HasColumnType("decimal(38,0)");
        builder.Property(x => x.CardType).HasColumnName("GC_CRD_TYP").HasMaxLength(1).IsFixedLength();
        builder.Property(x => x.EnteredByUser).HasColumnName("GC_ENT_USR").HasColumnType("decimal(38,0)");
        builder.Property(x => x.EnteredOn).HasColumnName("GC_ENT_DAT");
        builder.Property(x => x.EffectiveDate).HasColumnName("GC_EFF_DAT");
        builder.Property(x => x.ClosingDate).HasColumnName("GC_CLS_DAT");

        builder.HasIndex(x => x.CardSequence).HasDatabaseName("IDX_GUEST_CARD_MASTER_GC_CRD_SEQ");

        builder.Ignore(x => x.Id);
        builder.Ignore(x => x.IsActive);
        builder.Ignore(x => x.DomainEvents);
    }
}
