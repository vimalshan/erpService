using MedicineManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineManagement.Infrastructure.Persistence.Configurations;

public class PurchaseSubConfiguration : IEntityTypeConfiguration<PurchaseSub>
{
    public void Configure(EntityTypeBuilder<PurchaseSub> builder)
    {
        builder.ToTable("PURCHASE_SUB");
        builder.HasKey(e => new { e.CompanyCode, e.TransactionNumber, e.SerialNumber });
        builder.Property(e => e.CompanyCode).HasColumnName("MD_COM_COD").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(e => e.TransactionNumber).HasColumnName("MD_TRN_NUM").IsRequired();
        builder.Property(e => e.SerialNumber).HasColumnName("MD_SRL_NUM").HasMaxLength(255).IsRequired();
        builder.Property(e => e.MedicineCode).HasColumnName("MD_MED_COD").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(e => e.PackagingType).HasColumnName("MD_PKG_TYP").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(e => e.PackagingQuantity).HasColumnName("MD_PKG_QNT");
        builder.Property(e => e.PackagingNos).HasColumnName("MD_PKG_NOS");
        builder.Property(e => e.TotalQuantity).HasColumnName("MD_TOT_QNT");
        builder.Property(e => e.ManufacturingDate).HasColumnName("MD_MFG_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(e => e.ExpiryDate).HasColumnName("MD_EXP_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(e => e.LotNumber).HasColumnName("MD_LOT_NUM").HasMaxLength(50);
        builder.Property(e => e.EntryUser).HasColumnName("MD_ENT_USR").HasColumnType("CHAR(25)");
        builder.Property(e => e.EntryUserPin).HasColumnName("MD_USR_NUM").HasColumnType("DECIMAL(20,0)");
        builder.Property(e => e.EntryDate).HasColumnName("MD_ENT_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(e => e.ModifiedUser).HasColumnName("MD_MOD_USR").HasMaxLength(25);
        builder.Property(e => e.ModifiedUserPin).HasColumnName("MD_MOD_NUM").HasColumnType("DECIMAL(20,0)");
        builder.Property(e => e.ModifiedDate).HasColumnName("MD_MOD_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(e => e.CancelFlag).HasColumnName("MD_CAN_FLG").HasColumnType("CHAR(1)").IsRequired();

        builder.HasIndex(e => e.TransactionNumber).HasDatabaseName("IDX_PURCHASE_SUB_MD_TRN_NUM");
        builder.Ignore(e => e.DomainEvents);
    }
}
