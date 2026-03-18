using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ItemMasterService.Domain.Entities;

namespace ItemMasterService.Infrastructure.Persistence.EF.Configurations;

public class CanteenItemMasterConfiguration : IEntityTypeConfiguration<CanteenItemMaster>
{
    public void Configure(EntityTypeBuilder<CanteenItemMaster> builder)
    {
        builder.ToTable("CANTEEN_ITEM_MASTER");

        builder.HasKey(e => new { e.CanteenUnitCode, e.ItemCode });

        builder.Property(e => e.CanteenUnitCode).HasColumnName("CN_COM_COD").IsRequired();
        builder.Property(e => e.ItemCode).HasColumnName("CN_ITM_COD").IsRequired();
        builder.Property(e => e.ItemDescription).HasColumnName("CN_ITM_DES").HasColumnType("CHAR(50)").HasMaxLength(50);
        builder.Property(e => e.ItemType).HasColumnName("CN_ITM_TYP").HasColumnType("CHAR(1)").HasMaxLength(1);
        builder.Property(e => e.ItemReference).HasColumnName("CN_ITM_REF").HasColumnType("CHAR(10)").HasMaxLength(10);
        builder.Property(e => e.EnteredOn).HasColumnName("CN_ENT_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(e => e.EnteredBy).HasColumnName("CN_ENT_USR").HasColumnType("CHAR(50)").HasMaxLength(50);

        builder.Ignore(e => e.CreatedAt);
        builder.Ignore(e => e.UpdatedAt);
        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(e => e.Version);

        builder.HasMany(e => e.PriceMasters)
            .WithOne(p => p.ItemMaster)
            .HasForeignKey(p => new { p.CanteenUnitCode, p.ItemCode })
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CanteenItemPriceMasterConfiguration : IEntityTypeConfiguration<CanteenItemPriceMaster>
{
    public void Configure(EntityTypeBuilder<CanteenItemPriceMaster> builder)
    {
        builder.ToTable("CANTEEN_ITEM_PRICE_MASTER");

        // Composite key - use shadow property for Id (no natural single-column PK in SQL)
        builder.HasKey(e => new { e.CanteenUnitCode, e.ItemCode, e.EffectiveDate });

        builder.Property(e => e.CanteenUnitCode).HasColumnName("CN_COM_COD").IsRequired();
        builder.Property(e => e.ItemCode).HasColumnName("CN_ITM_COD").IsRequired();
        builder.Property(e => e.EmployeeContribution).HasColumnName("CN_EMP_CON").HasColumnType("DECIMAL(19,0)");
        builder.Property(e => e.EmployerContribution).HasColumnName("CN_EPR_CON").HasColumnType("DECIMAL(19,0)");
        builder.Property(e => e.EffectiveDate).HasColumnName("CN_EFF_DAT").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(e => e.ClosureDate).HasColumnName("CN_CLS_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(e => e.EnteredOn).HasColumnName("CN_ENT_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(e => e.EnteredBy).HasColumnName("CN_ENT_USR").HasColumnType("CHAR(50)").HasMaxLength(50);

        builder.Ignore(e => e.CreatedAt);
        builder.Ignore(e => e.UpdatedAt);
    }
}

public class CanteenGradeItemPriceConfiguration : IEntityTypeConfiguration<CanteenGradeItemPrice>
{
    public void Configure(EntityTypeBuilder<CanteenGradeItemPrice> builder)
    {
        builder.ToTable("CANTEENGRADE_ITEM_PRICE");

        builder.HasKey(e => e.CanteenUnitCode);

        builder.Property(e => e.CanteenUnitCode).HasColumnName("CN_COM_COD").IsRequired();
        builder.Property(e => e.ItemCode).HasColumnName("CN_ITM_COD");
        builder.Property(e => e.EmployeeContribution).HasColumnName("CN_EMP_CON").HasColumnType("DECIMAL(19,0)");
        builder.Property(e => e.EmployerContribution).HasColumnName("CN_EPR_CON").HasColumnType("DECIMAL(19,0)");
        builder.Property(e => e.EffectiveDate).HasColumnName("CN_EFF_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(e => e.ClosureDate).HasColumnName("CN_CLS_DAT").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(e => e.EnteredOn).HasColumnName("CN_ENT_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(e => e.EnteredBy).HasColumnName("CN_ENT_USR").HasColumnType("CHAR(50)").HasMaxLength(50).IsRequired();
        builder.Property(e => e.GradeType).HasColumnName("CN_GRD_TYP").HasColumnType("CHAR(3)").HasMaxLength(3).IsRequired();

        builder.Ignore(e => e.CreatedAt);
        builder.Ignore(e => e.UpdatedAt);
        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(e => e.Version);
    }
}
