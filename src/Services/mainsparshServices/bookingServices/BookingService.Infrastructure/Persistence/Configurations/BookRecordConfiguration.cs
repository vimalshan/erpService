using BookingService.Domain.Entities;
using BookingService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Persistence.Configurations;

public class BookRecordConfiguration : IEntityTypeConfiguration<BookRecord>
{
    public void Configure(EntityTypeBuilder<BookRecord> builder)
    {
        builder.ToTable("BOOK_REC");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("BOOK_REC_ID")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.BookingId).HasColumnName("BOOKING_ID").IsRequired();

        builder.Property(r => r.LocationCode)
            .HasColumnName("LOCATION_CODE")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.RecDetails)
            .HasColumnName("REC_DETAILS")
            .HasColumnType("nvarchar(max)");

        builder.Property(r => r.RecStatus)
            .HasColumnName("REC_STATUS")
            .HasMaxLength(20)
            .HasConversion(
                v => v.Value,
                v => RecordStatus.From(v))
            .HasDefaultValue(RecordStatus.Active);

        builder.Property(r => r.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(r => r.CreatedOn).HasColumnName("CREATED_ON").IsRequired();
        builder.Property(r => r.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(r => r.UpdatedOn).HasColumnName("UPDATED_ON");

        builder.HasIndex(r => r.BookingId);
        builder.HasIndex(r => r.LocationCode);

        builder.Ignore(r => r.DomainEvents);
    }
}
