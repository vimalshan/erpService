using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.TourPlan;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class TourPlanDaBreakConfiguration : IEntityTypeConfiguration<TourPlanDaBreak>
{
    public void Configure(EntityTypeBuilder<TourPlanDaBreak> builder)
    {
        builder.ToTable("TOURPLAN_DABREAK");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TPDA_ID").HasMaxLength(255);
        builder.Property(x => x.TourPlanId).HasColumnName("TPDA_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.CountryId).HasColumnName("TPDA_COUNTRYID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Currency).HasColumnName("TPDA_CURRENCY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Days).HasColumnName("TPDA_DAYS").HasPrecision(18, 4);
        builder.Property(x => x.Rate).HasColumnName("TPDA_RATE").HasPrecision(18, 4);
        builder.Property(x => x.GuestHouseDays).HasColumnName("TPDA_GHDAYS").HasPrecision(18, 4);
        builder.Property(x => x.GuestHouseRate).HasColumnName("TPDA_GHRATE").HasPrecision(18, 4);
        builder.Ignore(x => x.DomainEvents);
    }
}
