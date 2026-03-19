using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Infrastructure.Persistence.Configurations;

public class ProductionPlanConfiguration : IEntityTypeConfiguration<ProductionPlan>
{
    public void Configure(EntityTypeBuilder<ProductionPlan> builder)
    {
        builder.ToTable("PRODUCTION_PLAN");
        builder.HasKey(e => new { e.ProductionPlantId, e.SciItemId });

        builder.Property(e => e.ProductionPlantId)
            .HasColumnName("PRODUCTION_PLANT_ID");

        builder.Property(e => e.SciItemId)
            .HasColumnName("SCI_ITEM_ID");

        builder.Property(e => e.QtyPerDay)
            .HasColumnName("QTY_PERDAY")
            .IsRequired();

        builder.Property(e => e.PlanStartDate)
            .HasColumnName("PLAN_START_DATE")
            .HasColumnType("decimal(38)")
            .IsRequired();

        builder.Property(e => e.PlanClosureDate)
            .HasColumnName("PLAN_CLOSURE_DATE")
            .HasColumnType("datetime2(3)");

        builder.Property(e => e.SciUserIdModified)
            .HasColumnName("SCI_USER_ID_MODIFIED")
            .IsRequired();

        builder.Property(e => e.ModifiedDate)
            .HasColumnName("MODIFIED_DATE")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Ignore(e => e.DomainEvents);
    }
}
