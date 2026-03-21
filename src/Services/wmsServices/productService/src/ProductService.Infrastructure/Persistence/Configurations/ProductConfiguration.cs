using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(p => p.ProductId);
        builder.Property(p => p.ProductId).HasColumnName("product_id").ValueGeneratedOnAdd();
        builder.Property(p => p.Sku).HasColumnName("sku").HasMaxLength(50).IsRequired();
        builder.HasIndex(p => p.Sku).IsUnique();
        builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(p => p.Description).HasColumnName("description");
        builder.Property(p => p.CategoryId).HasColumnName("category_id");
        builder.Property(p => p.UnitOfMeasure).HasColumnName("unit_of_measure").HasMaxLength(20).HasDefaultValue("EA");
        builder.Property(p => p.WeightPerUnit).HasColumnName("weight_per_unit").HasColumnType("decimal(18,3)");
        builder.Property(p => p.VolumePerUnit).HasColumnName("volume_per_unit").HasColumnType("decimal(18,3)");
        builder.Property(p => p.Price).HasColumnName("price").HasColumnType("decimal(18,4)");
        builder.Property(p => p.ReorderPoint).HasColumnName("reorder_point").HasColumnType("decimal(18,3)");
        builder.Property(p => p.ReorderQuantity).HasColumnName("reorder_quantity").HasColumnType("decimal(18,3)");
        builder.Property(p => p.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(p => p.CreatedDate).HasColumnName("created_date").HasDefaultValueSql("GETDATE()");
        builder.Property(p => p.ModifiedDate).HasColumnName("modified_date").HasDefaultValueSql("GETDATE()");

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(p => p.Sku).HasDatabaseName("IX_Product_SKU");
        builder.HasIndex(p => p.Name).HasDatabaseName("IX_Product_Name");
        builder.HasIndex(p => p.CategoryId).HasDatabaseName("IX_Product_CategoryID");

        builder.Ignore(p => p.DomainEvents);
    }
}
