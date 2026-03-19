using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PurchaseSalesService.Domain.Entities;

namespace PurchaseSalesService.Infrastructure.Data.Configurations;

public sealed class LogSaleMainConfiguration : IEntityTypeConfiguration<LogSaleMain>
{
    public void Configure(EntityTypeBuilder<LogSaleMain> builder)
    {
        builder.ToTable("LOG_SALE_MAIN");
        builder.HasNoKey();

        builder.Property(x => x.SerialNumber).HasColumnName("SL_SER_NUM").IsRequired();
        builder.Property(x => x.TrackingNumber).HasColumnName("SL_TRC_NUM").IsRequired();
        builder.Property(x => x.TransactionNumber).HasColumnName("SL_TRN_NUM").IsRequired();
        builder.Property(x => x.PurposeCode).HasColumnName("SL_PUR_COD").IsRequired();
        builder.Property(x => x.StageCode).HasColumnName("SL_STG_COD").IsRequired();
        builder.Property(x => x.IsoNumber).HasColumnName("SL_ISO_NUM").HasMaxLength(25);
        builder.Property(x => x.IsoDate).HasColumnName("SL_ISO_DATE");
        builder.Property(x => x.ProductDescription).HasColumnName("SL_PRO_DES").HasMaxLength(100);
        builder.Property(x => x.UserId).HasColumnName("SL_USR_ID").HasMaxLength(25).IsRequired();
        builder.Property(x => x.UserNumber).HasColumnName("SL_USR_NUM").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("SL_UPD_DAT").IsRequired();
        builder.Property(x => x.CancelFlag).HasColumnName("SL_CAN_FLG").HasMaxLength(1)
            .HasConversion<string>(v => v.ToString()!, v => v[0]);
        builder.Property(x => x.ModifiedBy).HasColumnName("SL_MOD_USR").HasMaxLength(25).IsRequired();
        builder.Property(x => x.ModifiedByNumber).HasColumnName("SL_MOD_NUM").IsRequired();
        builder.Property(x => x.ModifiedAt).HasColumnName("SL_MOD_DAT").IsRequired();
    }
}
