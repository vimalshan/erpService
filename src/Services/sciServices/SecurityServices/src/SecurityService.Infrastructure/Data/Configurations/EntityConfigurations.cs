using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecurityService.Domain.Entities;

namespace SecurityService.Infrastructure.Data.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("USER_MASTER");
        builder.HasKey(u => u.UserId);
        builder.Property(u => u.UserId).HasColumnName("UM_USR_NUM").ValueGeneratedNever();

        builder.OwnsOne(u => u.UserCode, vc =>
        {
            vc.Property(v => v.Value)
              .HasColumnName("UM_USR_COD")
              .HasMaxLength(25)
              .IsRequired();
        });

        builder.OwnsOne(u => u.Email, em =>
        {
            em.Property(v => v.Value)
              .HasColumnName("UM_USR_MAI")
              .HasMaxLength(100);
        });

        builder.OwnsOne(u => u.Phone, ph =>
        {
            ph.Property(v => v.Value).HasColumnName("UM_USR_PHN");
        });

        builder.Property(u => u.UserName).HasColumnName("UM_USR_NAM").HasMaxLength(100);
        builder.Property(u => u.StartDate).HasColumnName("UM_STR_DAT").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(u => u.EndDate).HasColumnName("UM_END_DAT").HasColumnType("datetime2(3)");
        builder.Property(u => u.UserType).HasColumnName("UM_USR_TYP").HasColumnType("char(1)");
        builder.Property(u => u.UpdatedByCode).HasColumnName("UM_UPD_USR").HasMaxLength(25);
        builder.Property(u => u.UpdatedByNum).HasColumnName("UM_UPD_NUM");
        builder.Property(u => u.UpdatedAt).HasColumnName("UM_UPD_DAT").HasColumnType("datetime2(3)");

        builder.HasMany(u => u.UserRoles)
               .WithOne(ur => ur.User)
               .HasForeignKey(ur => ur.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("ROLE_MAST");
        builder.HasKey(r => r.RoleId);
        builder.Property(r => r.RoleId).HasColumnName("RL_ROL_COD").ValueGeneratedNever();
        builder.Property(r => r.RoleName).HasColumnName("RL_ROL_NAM").HasMaxLength(200).IsRequired();
        builder.Property(r => r.UpdatedByCode).HasColumnName("RL_UPD_USR").HasMaxLength(50);
        builder.Property(r => r.UpdatedByNum).HasColumnName("RL_UPD_NUM");
        builder.Property(r => r.UpdatedAt).HasColumnName("RL_UPD_DAT").HasColumnType("datetime2(3)");
    }
}

internal sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("USER_ROLE");
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });
        builder.Property(ur => ur.UserId).HasColumnName("UR_USR_NUM").IsRequired();
        builder.Property(ur => ur.RoleId).HasColumnName("UR_ROL_COD").IsRequired();
        builder.Property(ur => ur.StartDate).HasColumnName("UR_STR_DAT").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(ur => ur.EndDate).HasColumnName("UR_END_DAT").HasColumnType("datetime2(3)");
        builder.Property(ur => ur.UpdatedByCode).HasColumnName("UR_UPD_USR").HasMaxLength(25);
        builder.Property(ur => ur.UpdatedByNum).HasColumnName("UR_UPD_NUM");
        builder.Property(ur => ur.UpdatedAt).HasColumnName("UR_UPD_DAT").HasColumnType("datetime2(3)");

        builder.HasOne(ur => ur.Role)
               .WithMany()
               .HasForeignKey(ur => ur.RoleId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

internal sealed class AccessRoleConfiguration : IEntityTypeConfiguration<AccessRole>
{
    public void Configure(EntityTypeBuilder<AccessRole> builder)
    {
        builder.ToTable("ACCESS_ROLE").HasNoKey();
        builder.Property(e => e.UserCode).HasColumnName("RA_USR_COD").HasMaxLength(25);
        builder.Property(e => e.UserId).HasColumnName("RA_USR_NUM");
        builder.Property(e => e.RoleId).HasColumnName("RA_ROL_COD");
        builder.Property(e => e.UpdatedByCode).HasColumnName("RA_UPD_USR").HasMaxLength(25);
        builder.Property(e => e.UpdatedByNum).HasColumnName("RA_UPD_NUM");
        builder.Property(e => e.UpdatedAt).HasColumnName("RA_UPD_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.StartDate).HasColumnName("RA_STR_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.EndDate).HasColumnName("RA_END_DAT").HasColumnType("datetime2(3)");
    }
}

internal sealed class AccessRoleMasterConfiguration : IEntityTypeConfiguration<AccessRoleMaster>
{
    public void Configure(EntityTypeBuilder<AccessRoleMaster> builder)
    {
        builder.ToTable("ACCESS_ROLE_MASTER").HasNoKey();
        builder.Property(e => e.RoleId).HasColumnName("AR_ROL_COD");
        builder.Property(e => e.RoleName).HasColumnName("AR_ROL_NAM").HasMaxLength(200);
        builder.Property(e => e.UpdatedByCode).HasColumnName("AR_UPD_USR").HasMaxLength(25);
        builder.Property(e => e.UpdatedByNum).HasColumnName("AR_UPD_NUM");
        builder.Property(e => e.UpdatedAt).HasColumnName("AR_UPD_DAT").HasColumnType("datetime2(3)");
    }
}

internal sealed class AccessRoleMenuConfiguration : IEntityTypeConfiguration<AccessRoleMenu>
{
    public void Configure(EntityTypeBuilder<AccessRoleMenu> builder)
    {
        builder.ToTable("ACCESSROLE_MENU").HasNoKey();
        builder.Property(e => e.RoleId).HasColumnName("ARM_ROL_COD");
        builder.Property(e => e.MenuId).HasColumnName("ARM_MEN_COD");
        builder.Property(e => e.UpdatedByCode).HasColumnName("ARM_UPD_USR").HasMaxLength(25);
        builder.Property(e => e.UpdatedByNum).HasColumnName("ARM_UPD_NUM");
        builder.Property(e => e.UpdatedAt).HasColumnName("ARM_UPD_DAT").HasColumnType("datetime2(3)");
    }
}

internal sealed class MenuMasterConfiguration : IEntityTypeConfiguration<MenuMaster>
{
    public void Configure(EntityTypeBuilder<MenuMaster> builder)
    {
        builder.ToTable("MENUMASTER").HasNoKey();
        builder.Property(e => e.MenuId).HasColumnName("MENU_ID");
        builder.Property(e => e.MenuName).HasColumnName("MENU_NAME").HasMaxLength(100);
        builder.Property(e => e.Url).HasColumnName("URL").HasMaxLength(50);
        builder.Property(e => e.ParentMenuId).HasColumnName("PARENT_MENU_ID");
        builder.Property(e => e.DisplayOrder).HasColumnName("DISPLAYORDER");
    }
}

internal sealed class UserMasterMapConfiguration : IEntityTypeConfiguration<UserMasterMap>
{
    public void Configure(EntityTypeBuilder<UserMasterMap> builder)
    {
        builder.ToTable("USER_MASTER_MAP");
        builder.HasKey(e => e.MapId);
        builder.Property(e => e.MapId).HasColumnName("UM_MAP_ID").ValueGeneratedNever();
        builder.Property(e => e.UserId).HasColumnName("UM_USR_NUM").IsRequired();
        builder.Property(e => e.DepartmentCode).HasColumnName("UM_DEPT_COD").HasMaxLength(25).IsRequired();
        builder.Property(e => e.StartDate).HasColumnName("UM_STR_DAT").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.EndDate).HasColumnName("UM_END_DAT").HasColumnType("datetime2(3)");
    }
}
