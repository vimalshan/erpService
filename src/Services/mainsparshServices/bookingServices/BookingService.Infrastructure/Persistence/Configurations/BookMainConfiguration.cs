using BookingService.Domain.Entities;
using BookingService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Persistence.Configurations;

public class BookMainConfiguration : IEntityTypeConfiguration<BookMain>
{
    public void Configure(EntityTypeBuilder<BookMain> builder)
    {
        builder.ToTable("BOOK_MAIN");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .HasColumnName("BOOKING_ID")
            .ValueGeneratedOnAdd();

        builder.Property(b => b.BookingAppNo)
            .HasColumnName("BOOKING_APPNO")
            .HasMaxLength(50)
            .IsRequired();
        builder.HasIndex(b => b.BookingAppNo).IsUnique();

        builder.Property(b => b.BookingTitle)
            .HasColumnName("BOOKING_TITLE")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.LocationCode)
            .HasColumnName("LOCATION_CODE")
            .HasMaxLength(50);

        builder.Property(b => b.BookingDate)
            .HasColumnName("BOOKING_DATE");

        builder.Property(b => b.Status)
            .HasColumnName("BOOKING_STATUS")
            .HasMaxLength(20)
            .HasConversion(
                v => v.Value,
                v => BookingStatus.From(v))
            .HasDefaultValue(BookingStatus.Draft);

        builder.Property(b => b.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(b => b.CreatedOn).HasColumnName("CREATED_ON").IsRequired();
        builder.Property(b => b.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(b => b.UpdatedOn).HasColumnName("UPDATED_ON");

        builder.HasMany(b => b.Records)
            .WithOne()
            .HasForeignKey(r => r.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Attendees)
            .WithOne()
            .HasForeignKey(a => a.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(b => b.DomainEvents);
    }
}
