using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnitService.Domain.Entities;
using UnitService.Domain.ValueObjects;

namespace UnitService.Infrastructure.Data.Configurations;

public class CategoryMasterConfiguration : IEntityTypeConfiguration<CategoryMaster>
{
    public void Configure(EntityTypeBuilder<CategoryMaster> builder)
    {
        builder.ToTable("UM_CATEGORY_MASTER");

        builder.HasKey(e => e.UnitCode);
        builder.Property(e => e.UnitCode).HasColumnName("UNIT_CODE").HasMaxLength(3).IsRequired()
            .HasConversion(v => v.Value, v => UnitCode.From(v));
        builder.Property(e => e.CategoryId).HasColumnName("CATEGORY_ID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CategoryName).HasColumnName("CATEGORY_NAME").HasMaxLength(65).IsRequired();
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("LAST_MODIFIEDON").HasPrecision(3);
    }
}
