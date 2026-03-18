using MedicineManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineManagement.Infrastructure.Persistence.Configurations;

public class PurchaseMainConfiguration : IEntityTypeConfiguration<PurchaseMain>
{
    public void Configure(EntityTypeBuilder<PurchaseMain> builder)
    {
        builder.ToTable("PURCHASE_MAIN");
        builder.HasKey(e => new { e.CompanyCode, e.TransactionNumber });
        builder.Property(e => e.CompanyCode).HasColumnName("MD_COM_COD").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(e => e.TransactionNumber).HasColumnName("MD_TRN_NUM").IsRequired();
        builder.Property(e => e.VendorName).HasColumnName("MD_VND_NAM").HasMaxLength(100).IsRequired();
        builder.Property(e => e.InvoiceNumber).HasColumnName("MD_INV_NUM").HasMaxLength(30).IsRequired();
        builder.Property(e => e.InvoiceDate).HasColumnName("MD_INV_DAT").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(e => e.InvoiceAmount).HasColumnName("MD_INV_AMT").HasColumnType("DECIMAL(38)").IsRequired();
        builder.Property(e => e.EntryUser).HasColumnName("MD_ENT_USR").HasMaxLength(25).IsRequired();
        builder.Property(e => e.EntryUserPin).HasColumnName("MD_USR_NUM").HasColumnType("DECIMAL(20,0)").IsRequired();
        builder.Property(e => e.EntryDate).HasColumnName("MD_ENT_DAT").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(e => e.CancelFlag).HasColumnName("MD_CAN_FLG").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(e => e.ModifiedUser).HasColumnName("MD_MOD_USR").HasMaxLength(25);
        builder.Property(e => e.ModifiedUserPin).HasColumnName("MD_MOD_NUM").HasColumnType("DECIMAL(20,0)");
        builder.Property(e => e.ModifiedDate).HasColumnName("MD_MOD_DAT").HasColumnType("DATETIME2(3)");

        builder.HasMany(e => e.LineItems)
            .WithOne(e => e.PurchaseMain)
            .HasForeignKey(e => new { e.CompanyCode, e.TransactionNumber })
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.CompanyCode).HasDatabaseName("IDX_PURCHASE_MAIN_MD_COM_COD");
        builder.HasIndex(e => e.InvoiceDate).HasDatabaseName("IDX_PURCHASE_MAIN_MD_INV_DAT");
        builder.Ignore(e => e.DomainEvents);
    }
}
