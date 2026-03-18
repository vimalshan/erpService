using BookingService.Domain.Entities;
using BookingService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Persistence.Configurations;

public class BookAttendeeConfiguration : IEntityTypeConfiguration<BookAttendee>
{
    public void Configure(EntityTypeBuilder<BookAttendee> builder)
    {
        builder.ToTable("BOOK_ATTENDEES");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasColumnName("ATTENDEE_ID")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.BookingId).HasColumnName("BOOKING_ID").IsRequired();
        builder.Property(a => a.AttendeeSysId).HasColumnName("ATTENDEE_SYSID").IsRequired();
        builder.Property(a => a.AttendeeSerial).HasColumnName("ATTENDEE_SERIAL").IsRequired();

        builder.Property(a => a.AttendanceStatus)
            .HasColumnName("ATTENDANCE_STATUS")
            .HasMaxLength(20)
            .HasConversion(
                v => v.Value,
                v => AttendanceStatus.From(v))
            .HasDefaultValue(AttendanceStatus.Registered);

        builder.Property(a => a.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(a => a.CreatedOn).HasColumnName("CREATED_ON").IsRequired();
        builder.Property(a => a.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(a => a.UpdatedOn).HasColumnName("UPDATED_ON");

        builder.HasIndex(a => new { a.BookingId, a.AttendeeSysId }).IsUnique();
        builder.HasIndex(a => a.BookingId);
        builder.HasIndex(a => a.AttendeeSysId);

        builder.Ignore(a => a.DomainEvents);
    }
}
