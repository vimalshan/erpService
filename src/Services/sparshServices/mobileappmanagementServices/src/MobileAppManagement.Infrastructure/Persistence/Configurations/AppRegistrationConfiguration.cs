using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileAppManagement.Domain.Entities;

namespace MobileAppManagement.Infrastructure.Persistence.Configurations;

public class AppRegistrationConfiguration : IEntityTypeConfiguration<AppRegistration>
{
    public void Configure(EntityTypeBuilder<AppRegistration> builder)
    {
        builder.ToTable("MOBAPP_REGISTER");

        builder.HasKey(e => e.RegistrationId);

        builder.Property(e => e.RegistrationId)
            .HasColumnName("REGISTER_ID")
            .ValueGeneratedNever();

        builder.Property(e => e.EmployeeSysId)
            .HasColumnName("REGISTER_EMPSYSID");

        builder.Property(e => e.UserId)
            .HasColumnName("REGISTER_USERID")
            .HasMaxLength(255);

        builder.Property(e => e.UserSysId)
            .HasColumnName("REGISTER_USERSYSID");

        builder.Property(e => e.UserType)
            .HasColumnName("REGISTER_USERTYPE")
            .HasColumnType("char(1)");

        builder.Property(e => e.PinNo)
            .HasColumnName("REGISTER_PINNO");

        builder.Property(e => e.PinGeneratedOn)
            .HasColumnName("REGISTER_PINGENERATEDON")
            .HasColumnType("datetime2(3)");

        builder.Property(e => e.UpdatedOn)
            .HasColumnName("REGISTER_UPDATEDON")
            .HasColumnType("datetime2(3)");

        builder.Property(e => e.Status)
            .HasColumnName("REGISTER_STATUS")
            .HasColumnType("char(1)");

        builder.Property(e => e.MobileNo)
            .HasColumnName("REGISTER_MOBILENO")
            .HasMaxLength(255);

        builder.Property(e => e.ImeiNo)
            .HasColumnName("REGISTER_IMEINO")
            .HasMaxLength(255);

        builder.Property(e => e.Guid)
            .HasColumnName("REGISTER_GUID")
            .HasColumnType("char(1)");

        builder.Property(e => e.DeviceId)
            .HasColumnName("REGISTER_DEVICEID")
            .HasMaxLength(255);

        builder.Property(e => e.DeviceType)
            .HasColumnName("REGISTER_DTYPE")
            .HasColumnType("char(1)");

        builder.HasIndex(e => e.Status).HasDatabaseName("IX_MOBAPP_REG_STATUS");
        builder.HasIndex(e => e.UserId).HasDatabaseName("IX_MOBAPP_REG_USERID");

        builder.Ignore(e => e.DomainEvents);
    }
}
