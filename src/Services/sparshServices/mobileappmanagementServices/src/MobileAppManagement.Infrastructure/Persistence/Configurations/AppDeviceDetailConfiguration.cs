using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileAppManagement.Domain.Entities;

namespace MobileAppManagement.Infrastructure.Persistence.Configurations;

public class AppDeviceDetailConfiguration : IEntityTypeConfiguration<AppDeviceDetail>
{
    public void Configure(EntityTypeBuilder<AppDeviceDetail> builder)
    {
        builder.ToTable("MOB_APPDEVICE_DETAILS");

        builder.HasKey(e => new { e.EmployeeSysId, e.DeviceId });

        builder.Property(e => e.EmployeeSysId)
            .HasColumnName("MD_EMPSYSID")
            .HasColumnType("decimal(38,0)");

        builder.Property(e => e.DeviceId)
            .HasColumnName("MD_DEVICEID")
            .HasMaxLength(200);

        builder.Property(e => e.Active)
            .HasColumnName("MD_ACTIVE")
            .HasColumnType("char(1)")
            .IsRequired();

        builder.Property(e => e.DeviceType)
            .HasColumnName("MD_DEVICETYPE")
            .HasColumnType("char(1)");

        builder.Property(e => e.ImeiNo)
            .HasColumnName("MD_IMEINO")
            .HasMaxLength(200);

        builder.Property(e => e.CreatedOn)
            .HasColumnName("MD_CREATEDON")
            .HasColumnType("datetime2(3)");

        builder.Property(e => e.UpdatedBy)
            .HasColumnName("MD_UPDATEDBY")
            .HasColumnType("decimal(38,0)");

        builder.Property(e => e.UpdatedOn)
            .HasColumnName("MD_UPDATEDON")
            .HasColumnType("datetime2(3)");

        builder.HasIndex(e => e.Active).HasDatabaseName("IX_MOB_APPDEVICE_ACTIVE");
        builder.HasIndex(e => e.DeviceId).HasDatabaseName("IX_MOB_APPDEVICE_DEVICE");

        builder.Ignore(e => e.DomainEvents);
    }
}
