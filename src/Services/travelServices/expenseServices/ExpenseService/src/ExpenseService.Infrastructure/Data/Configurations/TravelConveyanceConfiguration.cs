using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseService.Infrastructure.Data.Configurations;

public class TravelConveyanceConfiguration : IEntityTypeConfiguration<TravelConveyance>
{
    public void Configure(EntityTypeBuilder<TravelConveyance> builder)
    {
        builder.ToTable("TRAVEL_CONVEYANCE");
        builder.HasKey(e => new { e.SerialNumber, e.RequestNumber });

        builder.Property(e => e.SerialNumber).HasColumnName("CONV_SRLNO");
        builder.Property(e => e.RequestNumber).HasColumnName("CONV_REQNO");
        builder.Property(e => e.Date).HasColumnName("CONV_DATE");
        builder.Property(e => e.Particulars).HasColumnName("CONV_PARTICULARS").HasMaxLength(255);
        builder.Property(e => e.Mode).HasColumnName("CONV_MODE");
        builder.Property(e => e.Amount).HasColumnName("CONV_AMOUNT");
        builder.Property(e => e.BookRequestNumber).HasColumnName("CONV_BOOKNUM");
        builder.Property(e => e.BookStatus).HasColumnName("CONV_BOOKSTS").HasMaxLength(255);

        builder.Ignore(e => e.DomainEvents);
    }
}
