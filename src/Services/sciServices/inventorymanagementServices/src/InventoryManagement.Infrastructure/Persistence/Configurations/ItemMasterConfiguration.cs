using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Persistence.Configurations;

public class ItemMasterConfiguration : IEntityTypeConfiguration<ItemMaster>
{
    public void Configure(EntityTypeBuilder<ItemMaster> builder)
    {
        builder.ToTable("ITEM_MASTER");
        builder.HasKey(x => x.SciItemId);
        builder.Property(x => x.SciItemId).HasColumnName("SCI_ITEM_ID");
        builder.Property(x => x.OracleCode).HasColumnName("ORACLE_CODE").HasMaxLength(20).IsRequired();
        builder.Property(x => x.OracleItemId).HasColumnName("ORACLE_ITEM_ID").IsRequired();
        builder.Property(x => x.MainProductId).HasColumnName("MAIN_PRODUCT_ID");
        builder.Property(x => x.ItemName).HasColumnName("ITEM_NAME").HasMaxLength(100);
        builder.Property(x => x.OracleDescription).HasColumnName("ORACLE_DESCRIPTION").HasMaxLength(200);
        builder.Property(x => x.ItemType).HasColumnName("ITEM_TYPE").HasMaxLength(20).IsRequired();
        builder.Property(x => x.PackageTypeId).HasColumnName("PACKAGE_TYPE_ID");
        builder.Property(x => x.ItemUomId).HasColumnName("ITEM_UOM_ID").IsRequired();
        builder.Property(x => x.MainProductUomConvFactor).HasColumnName("MAIN_PRODUCT_UOM_CONFACTOR").HasColumnType("decimal(38,0)");
        builder.Property(x => x.IsBulkSource).HasColumnName("ISBULK_SOURCE").HasColumnType("varchar(1)").IsRequired();
        builder.Property(x => x.IsBulkItem).HasColumnName("ISBULK_ITEM").HasColumnType("char(1)").IsRequired();
        builder.Property(x => x.MaterialTaxClassId).HasColumnName("MATERIAL_TAXCLASS");
        builder.Property(x => x.ProductClass).HasColumnName("PRODUCT_CLASS").HasColumnType("char(2)");
        builder.Property(x => x.EffectiveDate).HasColumnName("EFFECTIVE_DATE").HasMaxLength(255);
        builder.Property(x => x.ClosureDate).HasColumnName("CLOSURE_DATE").HasMaxLength(255);
        builder.Property(x => x.ModifiedBy).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE").HasMaxLength(255);
        builder.Property(x => x.LeadTime).HasColumnName("LEAD_TIME");
        builder.Property(x => x.ItemCapacityId).HasColumnName("ITEM_CAPACITY_ID");
        builder.Property(x => x.ItemUsage).HasColumnName("ITEM_USAGE").HasColumnType("char(2)");
        builder.Property(x => x.MamFlag).HasColumnName("MAM_FLAG").HasColumnType("char(1)");
        builder.Property(x => x.ItemAccType).HasColumnName("ITEM_ACC_TYPE").HasColumnType("char(1)");

        builder.HasOne(x => x.MainProduct).WithMany(x => x.Items)
            .HasForeignKey(x => x.MainProductId).IsRequired(false);
        builder.HasOne(x => x.PackageType).WithMany(x => x.Items)
            .HasForeignKey(x => x.PackageTypeId).IsRequired(false);
        builder.HasOne(x => x.UnitOfMeasure).WithMany()
            .HasForeignKey(x => x.ItemUomId);
    }
}
