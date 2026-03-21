using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelRequestService.Domain.Entities;

namespace TravelRequestService.Infrastructure.Data.Configurations;

public class TravelAgendaConfiguration : IEntityTypeConfiguration<TravelAgenda>
{
    public void Configure(EntityTypeBuilder<TravelAgenda> builder)
    {
        builder.ToTable("TRAVEL_AGENDA");

        builder.HasKey(e => new { e.SerialNumber, e.RequestNumber });

        builder.Property(e => e.RequestNumber).HasColumnName("TA_REQ_NUM").HasColumnType("bigint");
        builder.Property(e => e.SerialNumber).HasColumnName("TA_SRL_NO").HasColumnType("int");
        builder.Property(e => e.MeetingDate).HasColumnName("TA_MET_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.PeopleToMeet).HasColumnName("TA_MET_PPL").HasMaxLength(200);
        builder.Property(e => e.DesiredOutcome).HasColumnName("TA_OUT_COM").HasMaxLength(200);
        builder.Property(e => e.CityName).HasColumnName("TA_CITY_NAM").HasMaxLength(200);

        builder.Ignore(e => e.DomainEvents);
    }
}
