using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackingSystem.Domain.Entities;

namespace RackingSystem.Infrastructure.Persistence.Configurations;

public class RackConfiguration : IEntityTypeConfiguration<Rack>
{
    public void Configure(EntityTypeBuilder<Rack> builder)
    {
        builder.ToTable("Rack");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("rack_id").UseIdentityColumn();
        builder.Property(r => r.ZoneId).HasColumnName("zone_id").IsRequired();
        builder.Property(r => r.Code).HasColumnName("code").HasMaxLength(30).IsRequired();
        builder.Property(r => r.RackType).HasColumnName("rack_type").HasMaxLength(30);
        builder.Property(r => r.MaxLoadWeight).HasColumnName("max_load_weight").HasColumnType("decimal(18,3)");
        builder.Property(r => r.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(r => r.CreatedDate).HasColumnName("created_date").HasDefaultValueSql("GETDATE()");
        builder.Property(r => r.ModifiedDate).HasColumnName("modified_date").HasDefaultValueSql("GETDATE()");

        builder.HasIndex(r => new { r.ZoneId, r.Code }).IsUnique().HasDatabaseName("UQ_Rack_Zone_Code");
        builder.HasIndex(r => r.ZoneId).HasDatabaseName("IX_Rack_Zone");

        builder.HasMany(r => r.Shelves)
            .WithOne(s => s.Rack)
            .HasForeignKey(s => s.RackId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
