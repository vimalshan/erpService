using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackingSystem.Domain.Entities;

namespace RackingSystem.Infrastructure.Persistence.Configurations;

public class ShelfConfiguration : IEntityTypeConfiguration<Shelf>
{
    public void Configure(EntityTypeBuilder<Shelf> builder)
    {
        builder.ToTable("Shelf");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("shelf_id").UseIdentityColumn();
        builder.Property(s => s.RackId).HasColumnName("rack_id").IsRequired();
        builder.Property(s => s.ShelfLevel).HasColumnName("shelf_level").IsRequired();
        builder.Property(s => s.ShelfPosition).HasColumnName("shelf_position").IsRequired();
        builder.Property(s => s.Code).HasColumnName("code").HasMaxLength(30).IsRequired();
        builder.Property(s => s.CapacityQty).HasColumnName("capacity_qty").HasColumnType("decimal(18,3)");
        builder.Property(s => s.CapacityWeight).HasColumnName("capacity_weight").HasColumnType("decimal(18,3)");
        builder.Property(s => s.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(s => s.CreatedDate).HasColumnName("created_date").HasDefaultValueSql("GETDATE()");
        builder.Property(s => s.ModifiedDate).HasColumnName("modified_date").HasDefaultValueSql("GETDATE()");

        builder.HasIndex(s => new { s.RackId, s.ShelfLevel, s.ShelfPosition }).IsUnique()
            .HasDatabaseName("UQ_Shelf_Rack_Level_Position");
        builder.HasIndex(s => s.RackId).HasDatabaseName("IX_Shelf_Rack");
    }
}
