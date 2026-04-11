using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.TourPlan;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class TourPlanAgendaConfiguration : IEntityTypeConfiguration<TourPlanAgenda>
{
    public void Configure(EntityTypeBuilder<TourPlanAgenda> builder)
    {
        builder.ToTable("TOURPLAN_AGENDA");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("AGENDA_ID").HasMaxLength(255);
        builder.Property(x => x.TourPlanId).HasColumnName("AGENDA_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.City).HasColumnName("AGENDA_CITY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.PartyToMeet).HasColumnName("AGENDA_MEET").HasMaxLength(255).IsRequired();
        builder.Property(x => x.DesiredOutcome).HasColumnName("AGENDA_OUTCOME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AgendaDate).HasColumnName("AGENDA_TYPE");
        builder.Ignore(x => x.DomainEvents);
    }
}
