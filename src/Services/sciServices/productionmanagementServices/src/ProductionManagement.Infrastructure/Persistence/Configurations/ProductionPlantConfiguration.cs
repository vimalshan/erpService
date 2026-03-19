using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Infrastructure.Persistence.Configurations;

public class ProductionPlantConfiguration : IEntityTypeConfiguration<ProductionPlant>
{
    public void Configure(EntityTypeBuilder<ProductionPlant> builder)
    {
        builder.ToTable("PRODUCTION_PLANT");
        builder.HasKey(e => e.ProductionPlantId);

        builder.Property(e => e.ProductionPlantId)
            .HasColumnName("PRODUCTION_PLANT_ID")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CompanyUnitId)
            .HasColumnName("COMPANY_UNIT_ID")
            .IsRequired();

        builder.Property(e => e.PlantName)
            .HasColumnName("PLANT_NAME")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(e => e.Location)
            .HasColumnName("LOCATION")
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(e => e.SciUserIdCreated)
            .HasColumnName("SCI_USER_ID_CREATED");

        builder.Property(e => e.CreationDate)
            .HasColumnName("CREATION_DATE")
            .HasColumnType("datetime2(3)");

        builder.Property(e => e.SciUserIdModified)
            .HasColumnName("SCI_USER_ID_MODIFIED");

        builder.Property(e => e.ModifiedDate)
            .HasColumnName("MODIFIED_DATE")
            .HasColumnType("varchar(255)");

        builder.HasMany(e => e.ProductionPlans)
            .WithOne(e => e.ProductionPlant)
            .HasForeignKey(e => e.ProductionPlantId);

        builder.HasMany(e => e.ProductMaps)
            .WithOne(e => e.ProductionPlant)
            .HasForeignKey(e => e.ProductionPlantId);

        builder.Ignore(e => e.DomainEvents);
    }
}
