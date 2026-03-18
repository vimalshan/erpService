using BusServices.Domain.Entities;
using BusServices.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusServices.Infrastructure.Persistence.Configurations;

public sealed class BusConfiguration : IEntityTypeConfiguration<Bus>
{
    public void Configure(EntityTypeBuilder<Bus> builder)
    {
        builder.ToTable("BUS_MASTER");
        builder.HasKey(b => b.BusId);
        builder.Property(b => b.BusId).HasColumnName("BUS_ID").ValueGeneratedNever();
        builder.Property(b => b.Description).HasColumnName("BUS_DESCRIPTION").HasMaxLength(255);
        builder.Property(b => b.Capacity).HasColumnName("BUS_CAPACITY").IsRequired();
        builder.Property(b => b.CapacityReserved).HasColumnName("BUS_CAPACITY_RESERVED");
        builder.Property(b => b.OperatingFrom).HasColumnName("BUS_OPERATINGFROM").HasColumnType("datetime2(3)");
        builder.Property(b => b.LastModifiedBy).HasColumnName("BUS_LASTMODIFIEDBY").IsRequired();
        builder.Property(b => b.LastModifiedOn).HasColumnName("BUS_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.Property(b => b.RegistrationNumber)
            .HasConversion(
                rn => rn.Value,
                v => RegistrationNumber.Create(v))
            .HasColumnName("BUS_REGNUM")
            .HasMaxLength(50)
            .IsRequired();
        builder.HasIndex(b => b.RegistrationNumber).IsUnique();

        builder.Ignore(b => b.DomainEvents);

        builder.HasMany(b => b.Routes).WithOne().HasForeignKey("BusId").OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(b => b.Arrivals).WithOne().HasForeignKey("BusId").OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(b => b.DeductionRates).WithOne().HasForeignKey("BusId").OnDelete(DeleteBehavior.Restrict);
    }
}
