using MedicineManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineManagement.Infrastructure.Persistence.Configurations;

public class MedicineIssueConfiguration : IEntityTypeConfiguration<MedicineIssue>
{
    public void Configure(EntityTypeBuilder<MedicineIssue> builder)
    {
        builder.ToTable("MEDICINE_ISSUE");
        builder.HasKey(e => new { e.CompanyCode, e.TransactionNumber });
        builder.Property(e => e.CompanyCode).HasColumnName("MD_COM_COD").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(e => e.TransactionNumber).HasColumnName("MD_TRN_NUM").HasMaxLength(255).IsRequired();
        builder.Property(e => e.TransactionDate).HasColumnName("MD_TRN_DAT").HasMaxLength(255);
        builder.Property(e => e.IssuedQuantity).HasColumnName("MD_ISS_QNT");
        builder.Property(e => e.EntryUser).HasColumnName("MD_ENT_USR").HasMaxLength(25);
        builder.Property(e => e.EntryUserPin).HasColumnName("MD_USR_NUM").HasColumnType("DECIMAL(20,0)");
        builder.Property(e => e.EntryDate).HasColumnName("MD_ENT_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(e => e.VisitNumber).HasColumnName("MD_VIS_NUM").HasMaxLength(255);
        builder.Property(e => e.ModifiedUser).HasColumnName("MD_MOD_USR").HasMaxLength(25);
        builder.Property(e => e.ModifiedUserPin).HasColumnName("MD_MOD_NUM").HasColumnType("DECIMAL(20,0)");
        builder.Property(e => e.ModifiedDate).HasColumnName("MD_MOD_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(e => e.MedicineCode).HasColumnName("MD_MED_COD").HasColumnType("CHAR(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}
