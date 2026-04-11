using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.TourPlan;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class TourPlanNmsScheduleConfiguration : IEntityTypeConfiguration<TourPlanNmsSchedule>
{
    public void Configure(EntityTypeBuilder<TourPlanNmsSchedule> builder)
    {
        builder.ToTable("TOURPLAN_NMSSCH");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("NMSSCH_ID").HasMaxLength(255);
        builder.Property(x => x.TourPlanId).HasColumnName("NMSSCH_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.CityId).HasColumnName("NMSSCH_CITYID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.CityName).HasColumnName("NMSSCH_CITYNAME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.FromDate).HasColumnName("NMSSCH_FROMDATE").IsRequired();
        builder.Property(x => x.FromTime).HasColumnName("NMSSCH_FROMTIME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ToDate).HasColumnName("NMSSCH_TODATE").IsRequired();
        builder.Property(x => x.ToTime).HasColumnName("NMSSCH_TOTIME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NoDays).HasColumnName("NMSSCH_NODAYS").HasPrecision(18, 4);
        builder.Property(x => x.TravelModeId).HasColumnName("NMSSCH_MODEID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TravelClassId).HasColumnName("NMSSCH_CLASSID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Purpose).HasColumnName("NMSSCH_PURPOSE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Remarks).HasColumnName("NMSSCH_REMARKS").HasMaxLength(255).IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}
