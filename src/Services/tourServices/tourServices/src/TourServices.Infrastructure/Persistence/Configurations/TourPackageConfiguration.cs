using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourServices.Domain.Aggregates;
using TourServices.Domain.ValueObjects;

namespace TourServices.Infrastructure.Persistence.Configurations;

public sealed class TourPackageConfiguration : IEntityTypeConfiguration<TourPackage>
{
    public void Configure(EntityTypeBuilder<TourPackage> builder)
    {
        builder.ToTable("TOUR_PACKAGE");

        builder.HasKey(e => e.TourId);
        builder.Property(e => e.TourId)
            .HasColumnName("TOUR_ID")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.TourName)
            .HasColumnName("TOUR_NAME")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Destination)
            .HasColumnName("DESTINATION")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.StartDate)
            .HasColumnName("START_DATE")
            .IsRequired();

        builder.Property(e => e.EndDate)
            .HasColumnName("END_DATE")
            .IsRequired();

        builder.Property(e => e.TourPackageCost)
            .HasColumnName("TOUR_PACKAGE_COST")
            .HasColumnType("decimal(19,0)")
            .HasConversion(
                v => v.Amount,
                v => new Money(v))
            .IsRequired();

        builder.Property(e => e.MaxParticipants)
            .HasColumnName("MAX_PARTICIPANTS")
            .IsRequired();

        builder.Property(e => e.TourStatus)
            .HasColumnName("TOUR_STATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(
                v => v.Code,
                v => TourStatus.From(v));

        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").IsRequired();
        builder.Property(e => e.ModifiedBy).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedOn).HasColumnName("MODIFIED_ON");

        builder.HasMany(e => e.Registrations)
            .WithOne()
            .HasForeignKey(r => r.TourId)
            .HasConstraintName("FK_TOUR_REG_PACKAGE");

        builder.Ignore(e => e.DomainEvents);
    }
}
