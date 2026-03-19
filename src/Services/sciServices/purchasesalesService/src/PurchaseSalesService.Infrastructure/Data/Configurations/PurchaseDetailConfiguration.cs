using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PurchaseSalesService.Domain.Entities;

namespace PurchaseSalesService.Infrastructure.Data.Configurations;

public sealed class PurchaseDetailConfiguration : IEntityTypeConfiguration<PurchaseDetail>
{
    public void Configure(EntityTypeBuilder<PurchaseDetail> builder)
    {
        builder.ToTable("PURCHASE_DETAILS");
        builder.HasKey(x => x.SerialNumber);

        builder.Property(x => x.SerialNumber).HasColumnName("PD_SRL_NUM").ValueGeneratedOnAdd();
        builder.Property(x => x.TrackingNumber).HasColumnName("PD_TRC_NUM").IsRequired();
        builder.Property(x => x.TransactionNumber).HasColumnName("PD_TRN_NUM").IsRequired();
        builder.Property(x => x.PurposeCode).HasColumnName("PD_PUR_COD").IsRequired();
        builder.Property(x => x.StageCode).HasColumnName("PD_STG_COD").IsRequired();
        builder.Property(x => x.OracleMerchandise).HasColumnName("PD_ORA_MRC");
        builder.Property(x => x.SupplierCode).HasColumnName("PD_SUP_COD").HasMaxLength(25);
        builder.Property(x => x.TonNumLoaded).HasColumnName("PD_TON_NUM_LD").HasMaxLength(255);
        builder.Property(x => x.TonNumUnloaded).HasColumnName("PD_TON_NUM_UD").HasMaxLength(255);
        builder.Property(x => x.UserId).HasColumnName("PD_USR_ID").HasMaxLength(25);
        builder.Property(x => x.UserNumber).HasColumnName("PD_USR_NUM");
        builder.Property(x => x.UpdatedAt).HasColumnName("PD_UPD_DAT").IsRequired();
        builder.Property(x => x.CancelFlag).HasColumnName("PD_CAN_FLG").HasMaxLength(1)
            .HasConversion<string>(v => v.ToString()!, v => v[0]);
    }
}
