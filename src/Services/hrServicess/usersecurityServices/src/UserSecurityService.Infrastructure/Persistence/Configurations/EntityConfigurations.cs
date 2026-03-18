using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserSecurityService.Domain.Entities;

namespace UserSecurityService.Infrastructure.Persistence.Configurations;

public class UserAppsMappingConfiguration : IEntityTypeConfiguration<UserAppsMap>
{
    public void Configure(EntityTypeBuilder<UserAppsMap> builder)
    {
        builder.ToTable("USER_APPSMAP");
        builder.HasKey(x => x.UserEmpSysId);

        builder.Property(x => x.UserEmpSysId).HasColumnName("USER_EMPSYSID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.UserApps).HasColumnName("USER_APPS").HasMaxLength(20).IsRequired();
        builder.Property(x => x.UserEffDate).HasColumnName("USER_EFFDATE").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.UserClsDate).HasColumnName("USER_CLSDATE").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.UserModifiedBy).HasColumnName("USER_MODIFIEDBY").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.UserModifiedOn).HasColumnName("USER_MODIFIEDON").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.UserHrRoleId).HasColumnName("USER_HRROLEID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.UserCreatedBy).HasColumnName("USER_CREATEDBY").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.UserCreatedOn).HasColumnName("USER_CREATEDON").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.UserRemarks).HasColumnName("USER_REMARKS").HasMaxLength(200);

        builder.Ignore(x => x.DomainEvents);
    }
}

public class UserCalenderMapConfiguration : IEntityTypeConfiguration<UserCalenderMap>
{
    public void Configure(EntityTypeBuilder<UserCalenderMap> builder)
    {
        builder.ToTable("USER_CALENDERMAP");
        builder.HasKey(x => x.UserRoleId);

        builder.Property(x => x.UserRoleId).HasColumnName("USER_ROLEID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.CalendarId).HasColumnName("CALENDAR_ID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.ModifiedBy).HasColumnName("MODIFIEDBY").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.ModifiedOn).HasColumnName("MODIFIEDON").HasColumnType("DATETIME2(3)");

        builder.Ignore(x => x.DomainEvents);
    }
}

public class UserMenuMapConfiguration : IEntityTypeConfiguration<UserMenuMap>
{
    public void Configure(EntityTypeBuilder<UserMenuMap> builder)
    {
        builder.ToTable("USER_MENUMAP");
        builder.HasKey(x => x.UserRoleId);

        builder.Property(x => x.UserRoleId).HasColumnName("USER_ROLEID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.UserMenuId).HasColumnName("USER_MENUID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.UserModifiedBy).HasColumnName("USER_MODIFIEDBY").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.UserModifiedOn).HasColumnName("USER_MODIFIEDON").HasColumnType("DATETIME2(3)");

        builder.Ignore(x => x.DomainEvents);
    }
}

public class UserUnitMapConfiguration : IEntityTypeConfiguration<UserUnitMap>
{
    public void Configure(EntityTypeBuilder<UserUnitMap> builder)
    {
        builder.ToTable("USER_UNITMAP");
        builder.HasNoKey(); // No PK declared in schema

        builder.Property(x => x.RoleId).HasColumnName("ROLE_ID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.RoleApps).HasColumnName("ROLE_APPS").HasMaxLength(10);
        builder.Property(x => x.RoleEmpSysId).HasColumnName("ROLE_EMPSYSID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.RoleOrgId).HasColumnName("ROLE_ORGID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.RoleUnitAll).HasColumnName("ROLE_UNITALL").HasMaxLength(1).HasConversion<string>();
        builder.Property(x => x.RoleUnitId).HasColumnName("ROLE_UNITID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.RoleMenuGroupId).HasColumnName("ROLE_MENUGROUPID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.RoleType).HasColumnName("ROLE_TYPE").HasMaxLength(3);
        builder.Property(x => x.RoleEffDate).HasColumnName("ROLE_EFFDATE").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.RoleClsDate).HasColumnName("ROLE_CLSDATE").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.RoleModifiedBy).HasColumnName("ROLE_MODIFIEDBY").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.RoleModifiedOn).HasColumnName("ROLE_MODIFIEDON").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.RoleRemarks).HasColumnName("ROLE_REMARKS").HasMaxLength(200);
        builder.Property(x => x.RoleVtcEntry).HasColumnName("ROLE_VTCENTRY").HasMaxLength(1).HasConversion<string?>();

        builder.Ignore(x => x.DomainEvents);
    }
}

public class UserUnitMapLogConfiguration : IEntityTypeConfiguration<UserUnitMapLog>
{
    public void Configure(EntityTypeBuilder<UserUnitMapLog> builder)
    {
        builder.ToTable("USER_UNITMAPLOG");
        builder.HasNoKey();

        builder.Property(x => x.RoleId).HasColumnName("ROLE_ID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.RoleApps).HasColumnName("ROLE_APPS").HasMaxLength(10);
        builder.Property(x => x.RoleEmpSysId).HasColumnName("ROLE_EMPSYSID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.RoleOrgId).HasColumnName("ROLE_ORGID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.RoleUnitAll).HasColumnName("ROLE_UNITALL").HasMaxLength(1).HasConversion<string>();
        builder.Property(x => x.RoleUnitId).HasColumnName("ROLE_UNITID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.RoleMenuGroupId).HasColumnName("ROLE_MENUGROUPID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.RoleType).HasColumnName("ROLE_TYPE").HasMaxLength(3);
        builder.Property(x => x.RoleEffDate).HasColumnName("ROLE_EFFDATE").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.RoleClsDate).HasColumnName("ROLE_CLSDATE").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.RoleModifiedBy).HasColumnName("ROLE_MODIFIEDBY").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.RoleModifiedOn).HasColumnName("ROLE_MODIFIEDON").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.RoleRemarks).HasColumnName("ROLE_REMARKS").HasMaxLength(200);
        builder.Property(x => x.RoleVtcEntry).HasColumnName("ROLE_VTCENTRY").HasMaxLength(1).HasConversion<string?>();
        builder.Property(x => x.LogCreatedBy).HasColumnName("LOG_CREATEDBY").HasColumnType("DECIMAL(22,0)");
        builder.Property(x => x.LogCreatedOn).HasColumnName("LOG_CREATEDON").HasColumnType("DATETIME2(3)");

        builder.Ignore(x => x.DomainEvents);
    }
}

public class UserCalenderMapLogConfiguration : IEntityTypeConfiguration<UserCalenderMapLog>
{
    public void Configure(EntityTypeBuilder<UserCalenderMapLog> builder)
    {
        builder.ToTable("USER_CALENDERMAP_LOG");
        builder.HasNoKey();

        builder.Property(x => x.UserRoleId).HasColumnName("USER_ROLEID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.CalendarId).HasColumnName("CALENDAR_ID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.ClsDate).HasColumnName("CLSDATE").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.ModifiedBy).HasColumnName("MODIFIEDBY").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.ModifiedOn).HasColumnName("MODIFIEDON").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.LogCreatedBy).HasColumnName("LOGCREATED_BY").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.LogCreatedOn).HasColumnName("LOGCREATED_ON").HasColumnType("DATETIME2(3)");

        builder.Ignore(x => x.DomainEvents);
    }
}

public class UserMenuMapLogConfiguration : IEntityTypeConfiguration<UserMenuMapLog>
{
    public void Configure(EntityTypeBuilder<UserMenuMapLog> builder)
    {
        builder.ToTable("USER_MENUMAP_LOG");
        builder.HasNoKey();

        builder.Property(x => x.UserRoleId).HasColumnName("USER_ROLEID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.UserMenuId).HasColumnName("USER_MENUID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.UserModifiedBy).HasColumnName("USER_MODIFIEDBY").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.UserModifiedOn).HasColumnName("USER_MODIFIEDON").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.LogCreatedBy).HasColumnName("LOG_CREATEDBY").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.LogCreatedOn).HasColumnName("LOG_CREATEDON").HasColumnType("DATETIME2(3)");

        builder.Ignore(x => x.DomainEvents);
    }
}

public class EmpPasswordChangeConfiguration : IEntityTypeConfiguration<EmpPasswordChange>
{
    public void Configure(EntityTypeBuilder<EmpPasswordChange> builder)
    {
        builder.ToTable("EMP_PASSWORDCHANGE");
        builder.HasKey(x => x.EpwdId);

        builder.Property(x => x.EpwdId).HasColumnName("EPWD_ID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.EpwdEmpSysId).HasColumnName("EPWD_EMPSYSID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.EpwdCreatedBy).HasColumnName("EPWD_CREATEDBY").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.EpwdCreatedOn).HasColumnName("EPWD_CREATEDON").HasColumnType("DATETIME2(3)");

        builder.Ignore(x => x.DomainEvents);
    }
}
