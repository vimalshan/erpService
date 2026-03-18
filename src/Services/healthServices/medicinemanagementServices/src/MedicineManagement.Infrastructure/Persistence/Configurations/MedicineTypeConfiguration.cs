using MedicineManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineManagement.Infrastructure.Persistence.Configurations;

public class MedicineTypeConfiguration : IEntityTypeConfiguration<MedicineType>
{
    public void Configure(EntityTypeBuilder<MedicineType> builder)
    {
        builder.ToTable("MEDICINE_TYPMAST");
        builder.HasKey(e => e.TypeCode);
        builder.Property(e => e.TypeCode).HasColumnName("MT_TYP_COD").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(e => e.TypeName).HasColumnName("MT_TYP_NAM").HasMaxLength(30);
        builder.Property(e => e.EntryUser).HasColumnName("MT_ENT_USR").HasMaxLength(25);
        builder.Property(e => e.EntryUserPin).HasColumnName("MT_USR_NUM").HasColumnType("DECIMAL(20,0)");
        builder.Property(e => e.EntryDate).HasColumnName("MT_ENT_DAT").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(e => e.ModifiedUser).HasColumnName("MT_MOD_USR").HasMaxLength(25);
        builder.Property(e => e.ModifiedUserPin).HasColumnName("MT_MOD_NUM").HasColumnType("DECIMAL(20,0)");
        builder.Property(e => e.ModifiedDate).HasColumnName("MT_MOD_DAT").HasColumnType("DATETIME2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}
