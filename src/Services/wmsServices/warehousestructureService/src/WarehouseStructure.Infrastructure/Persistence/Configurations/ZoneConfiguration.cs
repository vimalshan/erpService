using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseStructure.Domain.Entities;

namespace WarehouseStructure.Infrastructure.Persistence.Configurations;

public class ZoneConfiguration : IEntityTypeConfiguration<Zone>
{
    public void Configure(EntityTypeBuilder<Zone> builder)
    {
        builder.ToTable("Zone");

        builder.HasKey(z => z.Id);
        builder.Property(z => z.Id).HasColumnName("zone_id");

        builder.Property(z => z.WarehouseId).HasColumnName("warehouse_id").IsRequired();
        builder.Property(z => z.Code).HasColumnName("code").IsRequired().HasMaxLength(20);
        builder.Property(z => z.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(z => z.ZoneTypeValue).HasColumnName("zone_type").IsRequired().HasMaxLength(30);
        builder.Property(z => z.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(z => z.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);
        builder.Property(z => z.CreatedDate).HasColumnName("created_date").IsRequired();
        builder.Property(z => z.ModifiedDate).HasColumnName("modified_date").IsRequired();

        builder.Ignore(z => z.ZoneId);

        builder.HasIndex(z => new { z.WarehouseId, z.Code }).IsUnique();
        builder.HasIndex(z => z.WarehouseId);
    }
}
