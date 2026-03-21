using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventoryService.Domain.Entities;

namespace InventoryService.Infrastructure.Persistence.Configurations;

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.ToTable("InventoryTransaction");

        builder.HasKey(t => t.TransactionId);
        builder.Property(t => t.TransactionId).ValueGeneratedOnAdd();

        builder.Property(t => t.ProductId).IsRequired();
        builder.Property(t => t.WarehouseId).IsRequired();

        builder.Property(t => t.TransactionType)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(t => t.QuantityChange)
            .HasColumnType("decimal(18,3)");

        builder.Property(t => t.ReferenceType).HasMaxLength(30);
        builder.Property(t => t.ReferenceNumber).HasMaxLength(50);
        builder.Property(t => t.CreatedBy).HasMaxLength(50);
        builder.Property(t => t.Comments).HasMaxLength(255);

        builder.Property(t => t.TransactionDate)
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(t => t.ProductId).HasDatabaseName("IX_InventoryTransaction_Product");
        builder.HasIndex(t => t.BinId).HasDatabaseName("IX_InventoryTransaction_Bin");
        builder.HasIndex(t => t.WarehouseId).HasDatabaseName("IX_InventoryTransaction_Warehouse");
        builder.HasIndex(t => t.TransactionDate).HasDatabaseName("IX_InventoryTransaction_Date");

        builder.Ignore(t => t.DomainEvents);
    }
}
