using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Persistence.Configurations;

public class ProductMasterConfiguration : IEntityTypeConfiguration<ProductMaster>
{
    public void Configure(EntityTypeBuilder<ProductMaster> builder)
    {
        builder.ToTable("PRODUCT_MASTER");
        builder.HasKey(x => x.ProductCode);
        builder.Property(x => x.ProductCode).HasColumnName("PM_PRO_COD").HasMaxLength(25);
        builder.Property(x => x.ProductDescription).HasColumnName("PM_PRO_DESC").HasMaxLength(255);
        builder.Property(x => x.OracleDescription).HasColumnName("PM_ORA_DES").HasMaxLength(255);
        builder.Property(x => x.UomCode).HasColumnName("PM_UOM_COD").HasMaxLength(255);
    }
}

public class ProductTypeMasterConfiguration : IEntityTypeConfiguration<ProductTypeMaster>
{
    public void Configure(EntityTypeBuilder<ProductTypeMaster> builder)
    {
        builder.ToTable("PRODUCT_TYPE_MASTER");
        builder.HasKey(x => x.ProductTypeId);
        builder.Property(x => x.ProductTypeId).HasColumnName("PRODUCT_TYPE_ID").ValueGeneratedNever();
        builder.Property(x => x.TypeName).HasColumnName("TYPE_NAME").HasMaxLength(20);
        builder.Property(x => x.TypeDescription).HasColumnName("TYPE_DESCRIPTION").HasMaxLength(50);
        builder.Property(x => x.CreatedBy).HasColumnName("SCI_USER_ID_CREATED");
        builder.Property(x => x.CreatedDate).HasColumnName("CREATION_DATE");
        builder.Property(x => x.ModifiedBy).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.HasData(
            new { ProductTypeId = 1, TypeName = "BULK", TypeDescription = "Bulk Products", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { ProductTypeId = 2, TypeName = "PACKED", TypeDescription = "Packed Products", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { ProductTypeId = 3, TypeName = "SERVICE", TypeDescription = "Service Items", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}

public class PackageTypeConfiguration : IEntityTypeConfiguration<PackageType>
{
    public void Configure(EntityTypeBuilder<PackageType> builder)
    {
        builder.ToTable("PACKAGE_TYPE");
        builder.HasKey(x => x.PackageTypeId);
        builder.Property(x => x.PackageTypeId).HasColumnName("PACKAGE_TYPE_ID").ValueGeneratedNever();
        builder.Property(x => x.PackageTypeName).HasColumnName("PACKAGE_TYPE_NAME").HasMaxLength(20);
        builder.Property(x => x.CreatedBy).HasColumnName("SCI_USER_ID_CREATED");
        builder.Property(x => x.CreatedDate).HasColumnName("CREATION_DATE");
        builder.Property(x => x.ModifiedBy).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.HasData(
            new { PackageTypeId = 1, PackageTypeName = "BAG", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { PackageTypeId = 2, PackageTypeName = "DRUM", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { PackageTypeId = 3, PackageTypeName = "CARTON", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { PackageTypeId = 4, PackageTypeName = "CYLINDER", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { PackageTypeId = 5, PackageTypeName = "PALLET", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}

public class UnitOfMeasureConfiguration : IEntityTypeConfiguration<UnitOfMeasure>
{
    public void Configure(EntityTypeBuilder<UnitOfMeasure> builder)
    {
        builder.ToTable("UNIT_OF_MEASURE");
        builder.HasKey(x => x.UnitId);
        builder.Property(x => x.UnitId).HasColumnName("UNIT_ID").ValueGeneratedNever();
        builder.Property(x => x.UnitCode).HasColumnName("UNIT_CODE").HasMaxLength(3).IsRequired();
        builder.Property(x => x.UnitOfMeasurement).HasColumnName("UNIT_OF_MEASURENT").HasMaxLength(25);
        builder.Property(x => x.UnitClassId).HasColumnName("UNIT_CLASS_ID");
        builder.Property(x => x.BaseUnitFlag).HasColumnName("BASE_UNIT_FLAG").HasColumnType("char(1)");
        builder.Property(x => x.Description).HasColumnName("DESCRIPTION").HasMaxLength(50);
        builder.Property(x => x.CreatedBy).HasColumnName("SCI_USER_ID_CREATED");
        builder.Property(x => x.CreatedDate).HasColumnName("CREATION_DATE");
        builder.Property(x => x.ModifiedBy).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.HasOne(x => x.UnitsClass).WithMany(x => x.Units)
            .HasForeignKey(x => x.UnitClassId);

        builder.HasData(
            new { UnitId = 1, UnitCode = "KG", UnitOfMeasurement = "Kilogram", UnitClassId = 1, BaseUnitFlag = 'Y', CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { UnitId = 2, UnitCode = "MT", UnitOfMeasurement = "Metric Ton", UnitClassId = 1, BaseUnitFlag = 'N', CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { UnitId = 3, UnitCode = "LTR", UnitOfMeasurement = "Litre", UnitClassId = 2, BaseUnitFlag = 'Y', CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { UnitId = 4, UnitCode = "EA", UnitOfMeasurement = "Each", UnitClassId = 3, BaseUnitFlag = 'Y', CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}

public class UnitsClassConfiguration : IEntityTypeConfiguration<UnitsClass>
{
    public void Configure(EntityTypeBuilder<UnitsClass> builder)
    {
        builder.ToTable("UNITS_CLASS");
        builder.HasKey(x => x.UnitsClassId);
        builder.Property(x => x.UnitsClassId).HasColumnName("UNITS_CLASS_ID").ValueGeneratedNever();
        builder.Property(x => x.UnitsClassName).HasColumnName("UNITS_CLASS").HasMaxLength(10);

        builder.HasData(
            new UnitsClass { UnitsClassId = 1, UnitsClassName = "WEIGHT" },
            new UnitsClass { UnitsClassId = 2, UnitsClassName = "VOLUME" },
            new UnitsClass { UnitsClassId = 3, UnitsClassName = "EACH" }
        );
    }
}

public class MaterialTaxClassConfiguration : IEntityTypeConfiguration<MaterialTaxClass>
{
    public void Configure(EntityTypeBuilder<MaterialTaxClass> builder)
    {
        builder.ToTable("MATERIAL_TAX_CLASS");
        builder.HasKey(x => x.MaterialTaxClassId);
        builder.Property(x => x.MaterialTaxClassId).HasColumnName("MATERIAL_TAXCLASS_ID").ValueGeneratedNever();
        builder.Property(x => x.Description).HasColumnName("DESCRIPTION").HasMaxLength(60);
        builder.Property(x => x.CreatedBy).HasColumnName("SCI_USER_ID_CREATED");
        builder.Property(x => x.CreatedDate).HasColumnName("CREATION_DATE");
        builder.Property(x => x.ModifiedBy).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.HasData(
            new { MaterialTaxClassId = 1, Description = "TAXABLE", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { MaterialTaxClassId = 2, Description = "EXEMPT", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { MaterialTaxClassId = 3, Description = "ZERO-RATED", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}

public class ItemCapacityConfiguration : IEntityTypeConfiguration<ItemCapacity>
{
    public void Configure(EntityTypeBuilder<ItemCapacity> builder)
    {
        builder.ToTable("ITEM_CAPACITY");
        builder.HasKey(x => x.CapacityId);
        builder.Property(x => x.CapacityId).HasColumnName("CAPACITY_ID").ValueGeneratedNever();
        builder.Property(x => x.CapacityName).HasColumnName("CAPACITY_NAME").HasMaxLength(20);
        builder.Property(x => x.CreatedBy).HasColumnName("SCI_USER_ID_CREATED");
        builder.Property(x => x.CreatedDate).HasColumnName("CREATION_DATE");
        builder.Property(x => x.ModifiedBy).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.HasData(
            new { CapacityId = 1, CapacityName = "SMALL", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { CapacityId = 2, CapacityName = "MEDIUM", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { CapacityId = 3, CapacityName = "LARGE", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}

public class ItemGradeConfiguration : IEntityTypeConfiguration<ItemGrade>
{
    public void Configure(EntityTypeBuilder<ItemGrade> builder)
    {
        builder.ToTable("ITEM_GRADE");
        builder.HasKey(x => x.ItemGradeId);
        builder.Property(x => x.ItemGradeId).HasColumnName("ITEM_GRADE_ID").ValueGeneratedNever();
        builder.Property(x => x.ItemGradeName).HasColumnName("ITEM_GRADE_NAME").HasMaxLength(20);
        builder.Property(x => x.CreatedBy).HasColumnName("SCI_USER_ID_CREATED");
        builder.Property(x => x.CreatedDate).HasColumnName("CREATION_DATE");
        builder.Property(x => x.ModifiedBy).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.HasData(
            new { ItemGradeId = 1, ItemGradeName = "GRADE-A", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { ItemGradeId = 2, ItemGradeName = "GRADE-B", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { ItemGradeId = 3, ItemGradeName = "GRADE-C", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}

public class GradeMasterConfiguration : IEntityTypeConfiguration<GradeMaster>
{
    public void Configure(EntityTypeBuilder<GradeMaster> builder)
    {
        builder.ToTable("GRADE_MASTER");
        builder.HasKey(x => x.GradeCode);
        builder.Property(x => x.GradeCode).HasColumnName("GM_GRD_COD").HasMaxLength(25);
        builder.Property(x => x.GradeDescription).HasColumnName("GM_GRD_DESC").HasMaxLength(200);
        builder.Property(x => x.ProductCode).HasColumnName("GM_PRO_COD").HasMaxLength(25);
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductCode);
    }
}

public class ItemMapConfiguration : IEntityTypeConfiguration<ItemMap>
{
    public void Configure(EntityTypeBuilder<ItemMap> builder)
    {
        builder.ToTable("ITEM_MAP");
        builder.HasKey(x => new { x.OspItemId, x.OspUomCode, x.ItemId, x.UomCode });
        builder.Property(x => x.OspItemId).HasColumnName("OSP_ITEM_ID");
        builder.Property(x => x.OspUomCode).HasColumnName("OSP_UOM_CODE").HasMaxLength(3);
        builder.Property(x => x.ItemId).HasColumnName("ITEM_ID");
        builder.Property(x => x.UomCode).HasColumnName("UOM_CODE").HasColumnType("decimal(38,0)");
        builder.Property(x => x.Quantity).HasColumnName("QUANTITY").HasMaxLength(3);
        builder.Property(x => x.OracleCode).HasColumnName("ORACLE_CODE").HasColumnType("decimal(20,0)");
    }
}

public class AdvanceLicenseMasterConfiguration : IEntityTypeConfiguration<AdvanceLicenseMaster>
{
    public void Configure(EntityTypeBuilder<AdvanceLicenseMaster> builder)
    {
        builder.ToTable("ADVLIC_MASTER");
        builder.HasKey(x => x.AdvLicId);
        builder.Property(x => x.AdvLicId).HasColumnName("ADVLIC_ID").ValueGeneratedNever();
        builder.Property(x => x.AdvLicNo).HasColumnName("ADVLIC_NO").HasMaxLength(40);
        builder.Property(x => x.AdvLicFg).HasColumnName("ADVLIC_FG");
        builder.Property(x => x.AdvLicEoAmt).HasColumnName("ADVLIC_EOAMT").HasColumnType("decimal(19,0)");
        builder.Property(x => x.AdvLicExpAmt).HasColumnName("ADVLIC_EXPAMT").HasColumnType("decimal(19,0)");
        builder.HasMany(x => x.Entitlements).WithOne(x => x.AdvanceLicense).HasForeignKey(x => x.AdvLicId);
    }
}

public class AdvanceLicenseEntitlementConfiguration : IEntityTypeConfiguration<AdvanceLicenseEntitlement>
{
    public void Configure(EntityTypeBuilder<AdvanceLicenseEntitlement> builder)
    {
        builder.ToTable("ADVLIC_ENTITLEMENT");
        builder.HasKey(x => new { x.AdvLicId, x.AdvLicEntitlement });
        builder.Property(x => x.AdvLicId).HasColumnName("ADVLIC_ID");
        builder.Property(x => x.AdvLicEntitlement).HasColumnName("ADVLIC_ENTITLERM");
    }
}

public class ItemTypeConfiguration : IEntityTypeConfiguration<ItemType>
{
    public void Configure(EntityTypeBuilder<ItemType> builder)
    {
        builder.ToTable("ITEM_TYPE");
        builder.HasKey(x => x.ItemTypeId);
        builder.Property(x => x.ItemTypeId).HasColumnName("ITEM_TYPE_ID").ValueGeneratedNever();
        builder.Property(x => x.ItemTypeCode).HasColumnName("ITEM_TYPE_CODE").HasMaxLength(2);
        builder.Property(x => x.Description).HasColumnName("DESCRIPTION").HasMaxLength(60);
        builder.Property(x => x.CreatedBy).HasColumnName("SCI_USER_ID_CREATED");
        builder.Property(x => x.CreatedDate).HasColumnName("CREATION_DATE");
        builder.Property(x => x.ModifiedBy).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.HasData(
            new { ItemTypeId = 1, ItemTypeCode = "RM", Description = "Raw Material", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { ItemTypeId = 2, ItemTypeCode = "FG", Description = "Finished Goods", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { ItemTypeId = 3, ItemTypeCode = "SF", Description = "Semi Finished", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { ItemTypeId = 4, ItemTypeCode = "SP", Description = "Spare Parts", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { ItemTypeId = 5, ItemTypeCode = "PM", Description = "Packing Material", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
