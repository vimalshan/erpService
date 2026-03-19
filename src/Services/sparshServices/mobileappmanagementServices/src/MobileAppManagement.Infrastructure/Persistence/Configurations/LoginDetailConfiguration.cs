using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileAppManagement.Domain.Entities;

namespace MobileAppManagement.Infrastructure.Persistence.Configurations;

public class LoginDetailConfiguration : IEntityTypeConfiguration<LoginDetail>
{
    public void Configure(EntityTypeBuilder<LoginDetail> builder)
    {
        builder.ToTable("MOB_LOGINDET");

        builder.HasKey(e => e.LoginId);

        builder.Property(e => e.LoginId)
            .HasColumnName("LD_LOGINID")
            .HasColumnType("decimal(38,0)");

        builder.Property(e => e.UserSysId)
            .HasColumnName("LD_USERSYSID")
            .HasColumnType("decimal(38,0)");

        builder.Property(e => e.DeviceId)
            .HasColumnName("LD_DEVICEID")
            .HasMaxLength(200);

        builder.Property(e => e.Logon)
            .HasColumnName("LD_LOGON")
            .HasColumnType("datetime2(3)");

        builder.Property(e => e.Guid)
            .HasColumnName("LD_GUID")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.ImeiNo)
            .HasColumnName("LD_IMEINO")
            .HasMaxLength(200);

        builder.Property(e => e.DeviceType)
            .HasColumnName("LD_DEVICETYPE")
            .HasColumnType("char(1)");

        builder.HasIndex(e => e.UserSysId).HasDatabaseName("IX_MOB_LOGIN_USERID");
        builder.HasIndex(e => e.DeviceId).HasDatabaseName("IX_MOB_LOGIN_DEVICE");
        builder.HasIndex(e => e.Logon).HasDatabaseName("IX_MOB_LOGIN_LOGON");

        builder.Ignore(e => e.DomainEvents);
    }
}
