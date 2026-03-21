using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventoryService.Domain.Entities;

namespace InventoryService.Infrastructure.Persistence.Configurations;

public class StockLevelConfiguration : IEntityTypeConfiguration<StockLevel>
{
    public void Configure(EntityTypeBuilder<StockLevel> builder)
    {
        builder.ToTable("StockLevel");

        builder.HasKey(s => s.StockLevelId);
        builder.Property(s => s.StockLevelId).ValueGeneratedOnAdd();

        builder.Property(s => s.ProductId).IsRequired();
        builder.Property(s => s.WarehouseId).IsRequired();
        builder.Property(s => s.BinId).IsRequired();

        builder.Property(s => s.QuantityOnHand)
            .HasColumnType("decimal(18,3)")
            .HasDefaultValue(0);

        builder.Property(s => s.QuantityAllocated)
            .HasColumnType("decimal(18,3)")
            .HasDefaultValue(0);

        builder.Property(s => s.QuantityReserved)
            .HasColumnType("decimal(18,3)")
            .HasDefaultValue(0);

        builder.Ignore(s => s.QuantityAvailable);

        builder.Property(s => s.LastUpdated)
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(s => s.LastCountDate)
            .HasColumnType("datetime2");

        builder.HasIndex(s => new { s.ProductId, s.BinId })
            .IsUnique()
            .HasDatabaseName("UQ_StockLevel_Product_Bin");

        builder.HasIndex(s => s.ProductId).HasDatabaseName("IX_StockLevel_Product");
        builder.HasIndex(s => s.BinId).HasDatabaseName("IX_StockLevel_Bin");
        builder.HasIndex(s => s.WarehouseId).HasDatabaseName("IX_StockLevel_Warehouse");

        builder.Ignore(s => s.DomainEvents);
    }
}
