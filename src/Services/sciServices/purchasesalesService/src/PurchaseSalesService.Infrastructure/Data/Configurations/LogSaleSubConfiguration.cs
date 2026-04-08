using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PurchaseSalesService.Domain.Entities;

namespace PurchaseSalesService.Infrastructure.Data.Configurations;

public sealed class LogSaleSubConfiguration : IEntityTypeConfiguration<LogSaleSub>
{
    public void Configure(EntityTypeBuilder<LogSaleSub> builder)
    {
        builder.ToTable("LOG_SALE_SUB");
        builder.HasKey(x => x.ReferenceNumber);

        builder.Property(x => x.ReferenceNumber).HasColumnName("SS_REF_NUM").IsRequired();
        builder.Property(x => x.SerialNumber).HasColumnName("SS_SER_NUM").IsRequired();
        builder.Property(x => x.ProductCode).HasColumnName("SS_PRO_COD").HasMaxLength(25).IsRequired();
        builder.Property(x => x.ProductQuantity).HasColumnName("SS_PRO_QTN").HasPrecision(38, 6);
        builder.Property(x => x.ProductGrade).HasColumnName("SS_PRO_GRD").HasMaxLength(25).IsRequired();
        builder.Property(x => x.UserComment).HasColumnName("SS_USR_COM").HasMaxLength(200);
        builder.Property(x => x.CheckbookInvoice).HasColumnName("SS_CHB_INV");
        builder.Property(x => x.CancelFlag).HasColumnName("SS_CAN_FLG").HasMaxLength(1)
            .HasConversion<string>(v => v.ToString()!, v => v[0]);
        builder.Property(x => x.ModifiedBy).HasColumnName("SS_MOD_USR").HasMaxLength(25).IsRequired();
        builder.Property(x => x.ModifiedByNumber).HasColumnName("SS_MOD_NUM").IsRequired();
        builder.Property(x => x.ModifiedAt).HasColumnName("SS_MOD_DAT").IsRequired();
    }
}
