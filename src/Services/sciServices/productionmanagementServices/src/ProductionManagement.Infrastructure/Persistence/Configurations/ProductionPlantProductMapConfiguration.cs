using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Infrastructure.Persistence.Configurations;

public class ProductionPlantProductMapConfiguration : IEntityTypeConfiguration<ProductionPlantProductMap>
{
    public void Configure(EntityTypeBuilder<ProductionPlantProductMap> builder)
    {
        builder.ToTable("PRODUCTIONPLANT_PRODUCT_MAP");
        builder.HasKey(e => new { e.ProductionPlantId, e.ProductId });

        builder.Property(e => e.ProductionPlantId)
            .HasColumnName("PRODUCTION_PLANT_ID");

        builder.Property(e => e.ProductId)
            .HasColumnName("PRODUCT_ID");

        builder.Property(e => e.SciUserIdCreated)
            .HasColumnName("SCI_USER_ID_CREATED")
            .IsRequired();

        builder.Property(e => e.CreationDate)
            .HasColumnName("CREATION_DATE")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Ignore(e => e.DomainEvents);
    }
}
