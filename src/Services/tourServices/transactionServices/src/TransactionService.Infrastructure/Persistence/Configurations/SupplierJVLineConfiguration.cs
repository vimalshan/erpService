using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public sealed class SupplierJVLineConfiguration : IEntityTypeConfiguration<SupplierJVLine>
{
    public void Configure(EntityTypeBuilder<SupplierJVLine> builder)
    {
        builder.ToTable("JVSUP_SUB");
        builder.HasKey(x => x.JvSubId);

        builder.Property(x => x.JvSubId).HasColumnName("JV_SUBID").ValueGeneratedNever();
        builder.Property(x => x.JvId).HasColumnName("JV_ID").IsRequired();
        builder.Property(x => x.JvBu).HasColumnName("JV_BU").HasMaxLength(25).IsRequired();
        builder.Property(x => x.JvAcCode).HasColumnName("JV_ACCODE").HasMaxLength(25).IsRequired();
        builder.Property(x => x.JvSubAcc).HasColumnName("JV_SUBACC").HasMaxLength(25).IsRequired();
        builder.Property(x => x.JvCcCode).HasColumnName("JV_CCCODE").HasMaxLength(25).IsRequired();
        builder.Property(x => x.JvProduct).HasColumnName("JV_PRODUCT").HasMaxLength(25).IsRequired();
        builder.Property(x => x.JvDcFlag).HasColumnName("JV_DCFLAG").HasMaxLength(1).IsRequired();
        builder.Property(x => x.JvTrnAmt).HasColumnName("JV_TRNAMT").HasColumnType("DECIMAL(19,0)").IsRequired();
        builder.Property(x => x.JvLoc).HasColumnName("JV_LOC").HasMaxLength(25).IsRequired();
        builder.Property(x => x.JvRemarks).HasColumnName("JV_REMARKS").HasMaxLength(25).IsRequired();
        builder.Property(x => x.JvLineFlag).HasColumnName("JV_LINEFLAG").HasMaxLength(1).IsRequired();
        builder.Property(x => x.JvCombinationId).HasColumnName("JV_COMBINATIONID").HasMaxLength(200).IsRequired();
        builder.Property(x => x.JvSubType).HasColumnName("JV_SUBTYPE").HasMaxLength(3).IsRequired();
        builder.Property(x => x.JvCombinationCode).HasColumnName("JV_COMBINATIONCODE").HasMaxLength(207);
        builder.Property(x => x.JvIutaBu).HasColumnName("JV_IUTABU").HasMaxLength(25).IsRequired();
        builder.Property(x => x.JvTpId).HasColumnName("JV_TPID").IsRequired();
        builder.Property(x => x.JvBatchSubId).HasColumnName("JV_BATCHSUBID").IsRequired();
        builder.Property(x => x.JvGstBu).HasColumnName("JV_GSTBU").HasMaxLength(25);
        builder.Property(x => x.JvGstAcCode).HasColumnName("JV_GSTACCODE").HasMaxLength(255);
        builder.Property(x => x.JvGstSubAcc).HasColumnName("JV_GSTSUBACC").HasMaxLength(255);
        builder.Property(x => x.JvGstCcCode).HasColumnName("JV_GSTCCCODE").HasMaxLength(255);
        builder.Property(x => x.JvGstProduct).HasColumnName("JV_GSTPRODUCT").HasMaxLength(255);
        builder.Property(x => x.JvGstLoc).HasColumnName("JV_GSTLOC").HasMaxLength(255);
        builder.Property(x => x.JvGstCombinationId).HasColumnName("JV_GSTCOMBINATIONID").HasMaxLength(255);
        builder.Property(x => x.JvGstCombinationCode).HasColumnName("JV_GSTCOMBINATIONCODE").HasMaxLength(255);
        builder.Property(x => x.JvInvNo).HasColumnName("JV_INVNO").HasMaxLength(255);
        builder.Property(x => x.JvInvDate).HasColumnName("JV_INVDATE");
        builder.Property(x => x.JvPayType).HasColumnName("JV_PAYTYPE").HasMaxLength(3);
        builder.Property(x => x.JvTpCat).HasColumnName("JV_TPCAT").HasMaxLength(3);
        builder.Property(x => x.JvClass).HasColumnName("JV_CLASS");
        builder.Property(x => x.JvBasRateAmt).HasColumnName("JV_BASRATEAMT").HasColumnType("DECIMAL(19,0)");

        builder.Ignore(x => x.DomainEvents);
    }
}
