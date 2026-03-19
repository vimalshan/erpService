using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Infrastructure.Persistence.Configurations;

public class MamProductionMapConfiguration : IEntityTypeConfiguration<MamProductionMap>
{
    public void Configure(EntityTypeBuilder<MamProductionMap> builder)
    {
        builder.ToTable("MAM_PRODUCTION_MAP");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.RmCode)
            .HasColumnName("RM_CODE");

        builder.Property(e => e.FgCode)
            .HasColumnName("FG_CODE");

        builder.Property(e => e.SlNo)
            .HasColumnName("SLNO")
            .HasColumnType("decimal(38)");

        builder.Ignore(e => e.DomainEvents);
    }
}
