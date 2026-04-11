using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.TourPlan;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class TourPlanCostCentreConfiguration : IEntityTypeConfiguration<TourPlanCostCentre>
{
    public void Configure(EntityTypeBuilder<TourPlanCostCentre> builder)
    {
        builder.ToTable("TOURPLAN_COSTCENTRE");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TPCOST_ID").HasMaxLength(255);
        builder.Property(x => x.TourPlanId).HasColumnName("TPCOST_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.BusinessUnit).HasColumnName("TPCOST_BUCODE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.CostCentreCode).HasColumnName("TPCOST_CCCODE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.SubAccountCode).HasColumnName("TPCOST_SUBACCCODE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ProductCode).HasColumnName("TPCOST_PRODUCTCODE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LocationSegment).HasColumnName("TPCOST_LOCSEGMENT").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AllocationPercentage).HasColumnName("TPCOST_ALLLPER").HasPrecision(18, 4);
        builder.Property(x => x.IsDefault).HasColumnName("TPCOST_DEFAULT").HasConversion<string>().HasMaxLength(1);
        builder.Ignore(x => x.DomainEvents);
    }
}
