using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseStructure.Domain.Entities;

namespace WarehouseStructure.Infrastructure.Persistence.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("Warehouse");

        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).HasColumnName("warehouse_id");

        builder.Property(w => w.Code).HasColumnName("code").IsRequired().HasMaxLength(20);
        builder.HasIndex(w => w.Code).IsUnique();

        builder.Property(w => w.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(w => w.AddressLine).HasColumnName("address").HasMaxLength(200);
        builder.Property(w => w.City).HasColumnName("city").HasMaxLength(50);
        builder.Property(w => w.State).HasColumnName("state").HasMaxLength(50);
        builder.Property(w => w.Country).HasColumnName("country").HasMaxLength(50);
        builder.Property(w => w.PostalCode).HasColumnName("postal_code").HasMaxLength(20);
        builder.Property(w => w.Phone).HasColumnName("phone").HasMaxLength(20);
        builder.Property(w => w.Email).HasColumnName("email").HasMaxLength(100);
        builder.Property(w => w.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);
        builder.Property(w => w.CreatedDate).HasColumnName("created_date").IsRequired();
        builder.Property(w => w.ModifiedDate).HasColumnName("modified_date").IsRequired();

        builder.Ignore(w => w.WarehouseId);

        builder.HasMany(w => w.Zones)
            .WithOne(z => z.Warehouse)
            .HasForeignKey(z => z.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
