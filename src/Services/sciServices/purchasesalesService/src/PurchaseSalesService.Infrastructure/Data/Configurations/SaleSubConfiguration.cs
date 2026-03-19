using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PurchaseSalesService.Domain.Entities;

namespace PurchaseSalesService.Infrastructure.Data.Configurations;

public sealed class SaleSubConfiguration : IEntityTypeConfiguration<SaleSub>
{
    public void Configure(EntityTypeBuilder<SaleSub> builder)
    {
        builder.ToTable("SALE_SUB");

        // SALE_SUB has no natural PK; add a shadow Id for EF navigation support
        builder.Property<long>("Id").ValueGeneratedOnAdd();
        builder.HasKey("Id");

        builder.Property(x => x.ReferenceNumber).HasColumnName("SS_REF_NUM");
        builder.Property(x => x.SerialNumber).HasColumnName("SS_SER_NUM");
        builder.Property(x => x.ProductCode).HasColumnName("SS_PRO_COD").HasMaxLength(255);
        builder.Property(x => x.ProductQuantity).HasColumnName("SS_PRO_QTN").HasPrecision(38, 6);
        builder.Property(x => x.ProductGrade).HasColumnName("SS_PRO_GRD").HasMaxLength(255);
        builder.Property(x => x.UserComment).HasColumnName("SS_USR_COM").HasMaxLength(255);
        builder.Property(x => x.CheckbookInvoice).HasColumnName("SS_CHB_INV").HasMaxLength(255);
        builder.Property(x => x.CancelFlag).HasColumnName("SS_CAN_FLG").HasMaxLength(1)
            .HasConversion<string>(v => v.ToString()!, v => v[0]);
    }
}
