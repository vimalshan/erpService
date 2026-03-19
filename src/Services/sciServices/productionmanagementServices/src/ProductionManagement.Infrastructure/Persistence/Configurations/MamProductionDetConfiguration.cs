using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Infrastructure.Persistence.Configurations;

public class MamProductionDetConfiguration : IEntityTypeConfiguration<MamProductionDet>
{
    public void Configure(EntityTypeBuilder<MamProductionDet> builder)
    {
        builder.ToTable("MAM_PRODUCTION_DET");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.ProductionNo)
            .HasColumnName("PRODUCTION_NO");

        builder.Property(e => e.ProductionDate)
            .HasColumnName("PRODUCTION_DATE")
            .HasColumnType("datetime2(3)");

        builder.Property(e => e.ProductionFg)
            .HasColumnName("PRODUCTION_FG");

        builder.Property(e => e.ProductionQty)
            .HasColumnName("PRODUCTION_QTY")
            .HasColumnType("decimal(19,0)");

        builder.Ignore(e => e.DomainEvents);
    }
}
