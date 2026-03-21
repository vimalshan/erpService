using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelRequestService.Domain.Entities;

namespace TravelRequestService.Infrastructure.Data.Configurations;

public class TravelPersonalConfiguration : IEntityTypeConfiguration<TravelPersonal>
{
    public void Configure(EntityTypeBuilder<TravelPersonal> builder)
    {
        builder.ToTable("TRAVEL_PERSONAL");

        builder.HasKey(e => e.SerialNumber);

        builder.Property(e => e.SerialNumber).HasColumnName("TRAVEL_SRLNO").HasColumnType("decimal(38)");
        builder.Property(e => e.RequestNumber).HasColumnName("TRAVEL_REQNUM").HasColumnType("decimal(38)");
        builder.Property(e => e.StartDate).HasColumnName("TRAVEL_STARTDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.EndDate).HasColumnName("TRAVEL_ENDDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.Reason).HasColumnName("TRAVEL_REASON").HasMaxLength(2000);
        builder.Property(e => e.Hours).HasColumnName("TRAVEL_HOURS").HasColumnType("decimal(19,0)");

        builder.Ignore(e => e.DomainEvents);
    }
}
