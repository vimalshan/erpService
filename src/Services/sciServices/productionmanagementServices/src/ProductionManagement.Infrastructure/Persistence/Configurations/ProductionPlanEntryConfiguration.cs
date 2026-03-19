using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Infrastructure.Persistence.Configurations;

public class ProductionPlanEntryConfiguration : IEntityTypeConfiguration<ProductionPlanEntry>
{
    public void Configure(EntityTypeBuilder<ProductionPlanEntry> builder)
    {
        builder.ToTable("PRODUCTIONPLAN_ENTRY");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.OracleCode)
            .HasColumnName("ORACLE_CODE")
            .HasMaxLength(50);

        builder.Property(e => e.Month)
            .HasColumnName("MONTH")
            .HasMaxLength(10);

        builder.Property(e => e.ProType)
            .HasColumnName("PRO_TYPE")
            .HasColumnType("char(1)");

        builder.Property(e => e.ProValue)
            .HasColumnName("PRO_VALUE");

        builder.Property(e => e.FactoryId)
            .HasColumnName("FACTORY_ID");

        builder.Property(e => e.Zone)
            .HasColumnName("ZONE")
            .HasMaxLength(10);

        builder.Property(e => e.ProYear)
            .HasColumnName("PRO_YEAR");

        builder.Ignore(e => e.DomainEvents);
    }
}
