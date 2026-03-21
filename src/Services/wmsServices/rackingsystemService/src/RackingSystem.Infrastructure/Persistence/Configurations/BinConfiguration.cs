using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackingSystem.Domain.Entities;

namespace RackingSystem.Infrastructure.Persistence.Configurations;

public class BinConfiguration : IEntityTypeConfiguration<Bin>
{
    public void Configure(EntityTypeBuilder<Bin> builder)
    {
        builder.ToTable("Bin");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("bin_id").UseIdentityColumn();
        builder.Property(b => b.ZoneId).HasColumnName("zone_id").IsRequired();
        builder.Property(b => b.ShelfId).HasColumnName("shelf_id");
        builder.Property(b => b.Code).HasColumnName("code").HasMaxLength(30).IsRequired();
        builder.Property(b => b.Barcode).HasColumnName("barcode").HasMaxLength(50);
        builder.Property(b => b.BinType).HasColumnName("bin_type").HasMaxLength(50);
        builder.Property(b => b.CapacityQty).HasColumnName("capacity_qty").HasColumnType("decimal(18,3)");
        builder.Property(b => b.CapacityWeight).HasColumnName("capacity_weight").HasColumnType("decimal(18,3)");
        builder.Property(b => b.CapacityVolume).HasColumnName("capacity_volume").HasColumnType("decimal(18,3)");
        builder.Property(b => b.Status).HasColumnName("status").HasMaxLength(20).HasDefaultValue("AVAILABLE").IsRequired();
        builder.Property(b => b.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(b => b.CreatedDate).HasColumnName("created_date").HasDefaultValueSql("GETDATE()");
        builder.Property(b => b.ModifiedDate).HasColumnName("modified_date").HasDefaultValueSql("GETDATE()");

        builder.HasIndex(b => new { b.ZoneId, b.Code }).IsUnique().HasDatabaseName("UQ_Bin_Zone_Code");
        builder.HasIndex(b => b.ZoneId).HasDatabaseName("IX_Bin_Zone");
        builder.HasIndex(b => b.ShelfId).HasDatabaseName("IX_Bin_Shelf");
        builder.HasIndex(b => b.Code).HasDatabaseName("IX_Bin_Code");
        builder.HasIndex(b => b.Barcode).HasDatabaseName("IX_Bin_Barcode");

        builder.HasOne(b => b.Shelf)
            .WithMany()
            .HasForeignKey(b => b.ShelfId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
