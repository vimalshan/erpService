using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SciTransactional.Domain.Entities;

namespace SciTransactional.Infrastructure.Persistence.Configurations;

public sealed class VehicleDirectEntryConfiguration : IEntityTypeConfiguration<VehicleDirectEntryEntity>
{
    public void Configure(EntityTypeBuilder<VehicleDirectEntryEntity> builder)
    {
        builder.ToTable("VEHICLE_DIRECT_ENTRY");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("VDE_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.TrackingNumber).HasColumnName("VDE_TRK_NUM");
        builder.Property(e => e.EnteredDate).HasColumnName("VDE_ENT_DAT").HasPrecision(3);
        builder.Property(e => e.EnteredUser).HasColumnName("VDE_ENT_USR").HasMaxLength(50);

        builder.Ignore(e => e.DomainEvents);

        builder.HasData(
            new { Id = 1L, TrackingNumber = (long?)1001L,
                EnteredDate = new DateTime?(new DateTime(2026, 3, 18, 0, 0, 0, DateTimeKind.Utc)),
                EnteredUser = "ADMIN" },
            new { Id = 2L, TrackingNumber = (long?)1002L,
                EnteredDate = new DateTime?(new DateTime(2026, 3, 18, 0, 0, 0, DateTimeKind.Utc)),
                EnteredUser = "GATE_USER" },
            new { Id = 3L, TrackingNumber = (long?)1003L,
                EnteredDate = new DateTime?(new DateTime(2026, 3, 19, 0, 0, 0, DateTimeKind.Utc)),
                EnteredUser = "WB_USER" }
        );
    }
}
