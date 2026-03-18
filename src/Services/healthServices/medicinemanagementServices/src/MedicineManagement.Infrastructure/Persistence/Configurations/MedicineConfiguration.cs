using MedicineManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineManagement.Infrastructure.Persistence.Configurations;

public class MedicineConfiguration : IEntityTypeConfiguration<Medicine>
{
    public void Configure(EntityTypeBuilder<Medicine> builder)
    {
        builder.ToTable("MEDICINE_MAST");
        builder.HasKey(e => e.MedicineCode);
        builder.Property(e => e.MedicineCode).HasColumnName("MM_MED_COD").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(e => e.MedicineName).HasColumnName("MM_MED_NAM").HasMaxLength(50).IsRequired();
        builder.Property(e => e.MedicineTypeCode).HasColumnName("MM_MED_TYP").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(e => e.Category).HasColumnName("MM_MED_CAT").HasColumnType("CHAR(1)");
        builder.Property(e => e.OrderLevelMin).HasColumnName("MM_ORD_MIN").HasColumnType("DECIMAL(20,0)");
        builder.Property(e => e.OrderLevelMax).HasColumnName("MM_ORD_MAX").HasColumnType("DECIMAL(20,0)");
        builder.Property(e => e.EntryUser).HasColumnName("MM_ENT_USR").HasMaxLength(25);
        builder.Property(e => e.EntryUserPin).HasColumnName("MM_USR_NUM").HasColumnType("DECIMAL(20,0)");
        builder.Property(e => e.EntryDate).HasColumnName("MM_ENT_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(e => e.ModifiedUser).HasColumnName("MM_MOD_USR").HasMaxLength(25);
        builder.Property(e => e.ModifiedUserPin).HasColumnName("MM_MOD_NUM").HasColumnType("DECIMAL(20,0)");
        builder.Property(e => e.ModifiedDate).HasColumnName("MM_MOD_DAT").HasColumnType("DATETIME2(3)");

        builder.HasOne(e => e.MedicineType)
            .WithMany()
            .HasForeignKey(e => e.MedicineTypeCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.MedicineCode).HasDatabaseName("IDX_MEDICINE_MAST_MM_MED_COD");
        builder.Ignore(e => e.DomainEvents);
    }
}
