using MedicineManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineManagement.Infrastructure.Persistence.Configurations;

public class MedicineCreditConfiguration : IEntityTypeConfiguration<MedicineCredit>
{
    public void Configure(EntityTypeBuilder<MedicineCredit> builder)
    {
        builder.ToTable("MEDICINE_CREDIT");
        builder.HasKey(e => new { e.CompanyCode, e.TransactionCode });
        builder.Property(e => e.CompanyCode).HasColumnName("MD_COM_COD").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(e => e.TransactionCode).HasColumnName("MD_TRN_COD").IsRequired();
        builder.Property(e => e.MedicineCode).HasColumnName("MD_MED_COD").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(e => e.RecordType).HasColumnName("MD_REC_TYP").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(e => e.Quantity).HasColumnName("MD_MED_QNT").IsRequired();
        builder.Property(e => e.TransactionDate).HasColumnName("MD_TRN_DAT").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(e => e.EntryUser).HasColumnName("MD_ENT_USR").HasMaxLength(25).IsRequired();
        builder.Property(e => e.EntryUserPin).HasColumnName("MD_USR_NUM").HasColumnType("DECIMAL(20,0)").IsRequired();
        builder.Property(e => e.EntryDate).HasColumnName("MD_ENT_DAT").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(e => e.ModifiedUser).HasColumnName("MD_MOD_USR").HasMaxLength(25);
        builder.Property(e => e.ModifiedUserPin).HasColumnName("MD_MOD_NUM").HasColumnType("DECIMAL(20,0)");
        builder.Property(e => e.ModifiedDate).HasColumnName("MD_MOD_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(e => e.LotNumber).HasColumnName("MD_LOT_NUM").HasMaxLength(50);
        builder.Property(e => e.CancelFlag).HasColumnName("MD_CAN_FLG").HasColumnType("CHAR(1)");
        builder.Property(e => e.TransactionNumber).HasColumnName("MD_TRN_NUM");

        builder.HasIndex(e => e.CompanyCode).HasDatabaseName("IDX_MEDICINE_CREDIT_MD_COM_COD");
        builder.HasIndex(e => e.TransactionDate).HasDatabaseName("IDX_MEDICINE_CREDIT_MD_TRN_DAT");

        builder.HasOne(e => e.Medicine)
            .WithMany()
            .HasForeignKey(e => e.MedicineCode);

        builder.Ignore(e => e.DomainEvents);
    }
}
