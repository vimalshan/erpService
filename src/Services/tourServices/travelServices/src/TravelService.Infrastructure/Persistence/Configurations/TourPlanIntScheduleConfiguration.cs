using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.TourPlan;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class TourPlanIntScheduleConfiguration : IEntityTypeConfiguration<TourPlanIntSchedule>
{
    public void Configure(EntityTypeBuilder<TourPlanIntSchedule> builder)
    {
        builder.ToTable("TOURPLAN_INTSCH");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("INTSCH_ID").HasMaxLength(255);
        builder.Property(x => x.TourPlanId).HasColumnName("INTSCH_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.FromDate).HasColumnName("INTSCH_FROMDATE").IsRequired();
        builder.Property(x => x.FromTime).HasColumnName("INTSCH_FROMTIME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.FromCityId).HasColumnName("INTSCH_FROMCITYID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.FromCity).HasColumnName("INTSCH_FROMCITY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.FromCountry).HasColumnName("INTSCH_FROMCOUNTRY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ToDate).HasColumnName("INTSCH_TODATE").IsRequired();
        builder.Property(x => x.ToTime).HasColumnName("INTSCH_TOTIME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ToCityId).HasColumnName("INTSCH_TOCITYID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ToCity).HasColumnName("INTSCH_TOCITY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ToCountry).HasColumnName("INTSCH_TOCOUNTRY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ApproximateCost).HasColumnName("INTSCH_APPROXCOST").HasPrecision(18, 4);
        builder.Ignore(x => x.DomainEvents);
    }
}
