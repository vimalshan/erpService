using MedicineManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineManagement.Infrastructure.Persistence.Configurations;

public class MedicinePackagingConfiguration : IEntityTypeConfiguration<MedicinePackaging>
{
    public void Configure(EntityTypeBuilder<MedicinePackaging> builder)
    {
        builder.ToTable("MEDICINE_PKG");
        builder.HasKey(e => e.PackagingCode);
        builder.Property(e => e.PackagingCode).HasColumnName("PK_PKG_COD").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(e => e.PackagingType).HasColumnName("PK_PKG_TYP").HasMaxLength(20);
        builder.Property(e => e.EntryUser).HasColumnName("PK_ENT_USR").HasMaxLength(25);
        builder.Property(e => e.EntryUserPin).HasColumnName("PK_USR_NUM").HasColumnType("DECIMAL(20,0)");
        builder.Property(e => e.EntryDate).HasColumnName("PK_ENT_DAT").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(e => e.ModifiedUser).HasColumnName("PK_MOD_USR").HasMaxLength(25);
        builder.Property(e => e.ModifiedUserPin).HasColumnName("PK_MOD_NUM").HasColumnType("DECIMAL(38)");
        builder.Property(e => e.ModifiedDate).HasColumnName("PK_MOD_DAT").HasColumnType("DATETIME2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}
