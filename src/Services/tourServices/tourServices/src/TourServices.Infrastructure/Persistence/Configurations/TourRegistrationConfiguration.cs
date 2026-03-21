using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourServices.Domain.Entities;
using TourServices.Domain.ValueObjects;

namespace TourServices.Infrastructure.Persistence.Configurations;

public sealed class TourRegistrationConfiguration : IEntityTypeConfiguration<TourRegistration>
{
    public void Configure(EntityTypeBuilder<TourRegistration> builder)
    {
        builder.ToTable("TOUR_REGISTRATION");

        builder.HasKey(e => e.RegistrationId);
        builder.Property(e => e.RegistrationId)
            .HasColumnName("REGISTRATION_ID")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.TourId)
            .HasColumnName("TOUR_ID")
            .IsRequired();

        builder.Property(e => e.ParticipantId)
            .HasColumnName("PARTICIPANT_ID")
            .IsRequired();

        builder.Property(e => e.RegistrationDate)
            .HasColumnName("REGISTRATION_DATE")
            .IsRequired();

        builder.Property(e => e.RegistrationStatus)
            .HasColumnName("REGISTRATION_STATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(
                v => v.Code,
                v => RegistrationStatus.From(v));

        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").IsRequired();
        builder.Property(e => e.ModifiedBy).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedOn).HasColumnName("MODIFIED_ON");

        builder.Ignore(e => e.DomainEvents);
    }
}
