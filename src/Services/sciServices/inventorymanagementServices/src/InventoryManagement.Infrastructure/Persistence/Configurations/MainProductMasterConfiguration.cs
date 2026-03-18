using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Persistence.Configurations;

public class MainProductMasterConfiguration : IEntityTypeConfiguration<MainProductMaster>
{
    public void Configure(EntityTypeBuilder<MainProductMaster> builder)
    {
        builder.ToTable("MAIN_PRODUCT_MASTER");
        builder.HasKey(x => x.ProductId);
        builder.Property(x => x.ProductId).HasColumnName("PRODUCT_ID");
        builder.Property(x => x.ProductName).HasColumnName("PRODUCT_NAME").HasMaxLength(20);
        builder.Property(x => x.ProductDescription).HasColumnName("PRODUCT_DESCRIPTION").HasMaxLength(100);
        builder.Property(x => x.UnitId).HasColumnName("UNIT_ID");
        builder.Property(x => x.ProductTypeId).HasColumnName("PRODUCT_TYPE_ID");
        builder.Property(x => x.CompanyUnitId).HasColumnName("COMPANY_UNIT_ID");
        builder.Property(x => x.CreatedBy).HasColumnName("SCI_USER_ID_CREATED");
        builder.Property(x => x.CreatedDate).HasColumnName("CREATION_DATE");
        builder.Property(x => x.ModifiedBy).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE").HasMaxLength(255);
        builder.Property(x => x.MamFlag).HasColumnName("MAM_FLAG").HasColumnType("char(1)");

        builder.HasOne(x => x.ProductType).WithMany(x => x.Products)
            .HasForeignKey(x => x.ProductTypeId).IsRequired(false);
        builder.HasOne(x => x.Unit).WithMany()
            .HasForeignKey(x => x.UnitId).IsRequired(false);

        builder.HasData(
            new { ProductId = 1, ProductName = "UREA", ProductDescription = "Urea Fertilizer", UnitId = (int?)1, ProductTypeId = (int?)1, CompanyUnitId = (int?)1, CreatedBy = (int?)1, CreatedDate = (DateTime?)new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
