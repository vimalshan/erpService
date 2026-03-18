namespace AccessService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using AccessService.Domain.Entities;

/// <summary>
/// Entity Framework DbContext for Access Management Service
/// </summary>
public class AccessServiceDbContext : DbContext
{
    public AccessServiceDbContext(DbContextOptions<AccessServiceDbContext> options) : base(options)
    {
    }

    public DbSet<UserMap> UserMaps { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<Menu> Menus { get; set; } = null!;
    public DbSet<UserMenuMap> UserMenuMaps { get; set; } = null!;
    public DbSet<SPARSHMenu> SPARSHMenus { get; set; } = null!;
    public DbSet<SPARSHMenuAccess> SPARSHMenuAccess { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure UserMap entity
        modelBuilder.Entity<UserMap>(entity =>
        {
            entity.ToTable("AIMS_USERMAP");
            entity.HasKey(e => e.EmployeeSystemId);
            entity.Property(e => e.EmployeeSystemId)
                .HasColumnName("USER_EMPSYSID")
                .ValueGeneratedNever();
            entity.Property(e => e.EffectiveDate)
                .HasColumnName("USER_EFFDATE");
            entity.Property(e => e.ClosureDate)
                .HasColumnName("USER_CLSDATE");
            entity.Property(e => e.ModifiedBy)
                .HasColumnName("USER_MODIFIEDBY");
            entity.Property(e => e.ModifiedOn)
                .HasColumnName("USER_MODIFIEDON");
        });

        // Configure UserRole entity
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("AIMS_USERROLE");
            entity.HasKey(e => e.RoleId);
            entity.Property(e => e.RoleId)
                .HasColumnName("ROLE_ID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.EmployeeSystemId)
                .HasColumnName("ROLE_EMPSYSID");
            entity.Property(e => e.RoleType)
                .HasColumnName("ROLE_TYPE")
                .HasMaxLength(1);
            entity.Property(e => e.MenuAccess)
                .HasColumnName("ROLE_MENUACCESS")
                .HasMaxLength(1);
            entity.Property(e => e.OrganizationId)
                .HasColumnName("ROLE_ORGID");
            entity.Property(e => e.UnitId)
                .HasColumnName("ROLE_UNITID");
            entity.Property(e => e.CalendarId)
                .HasColumnName("ROLE_CALENDARID");
            entity.Property(e => e.EffectiveDate)
                .HasColumnName("ROLE_EFFDATE");
            entity.Property(e => e.ClosureDate)
                .HasColumnName("ROLE_CLSDATE");
            entity.Property(e => e.ModifiedBy)
                .HasColumnName("ROLE_MODIFIEDBY");
            entity.Property(e => e.ModifiedOn)
                .HasColumnName("ROLE_MODIFIEDON");
        });

        // Configure Menu entity
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("MENU_MASTER");
            entity.HasKey(e => e.MenuId);
            entity.Property(e => e.MenuId)
                .HasColumnName("MENU_ID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasColumnName("Menu_NAME")
                .HasMaxLength(100);
            entity.Property(e => e.ParentMenuId)
                .HasColumnName("MENU_PARENTID");
            entity.Property(e => e.Path)
                .HasColumnName("Menu_PATH")
                .HasMaxLength(150);
            entity.Property(e => e.CalendarRole)
                .HasColumnName("MENU_CALENDARROLE")
                .HasMaxLength(1);
            entity.Property(e => e.Type)
                .HasColumnName("MENU_TYPE")
                .HasMaxLength(1);
            entity.Property(e => e.DisplayOrder)
                .HasColumnName("MENU_DISPLAYORDER");
            entity.Property(e => e.ModifiedBy)
                .HasColumnName("MENU_MODIFIEDBY");
            entity.Property(e => e.ModifiedOn)
                .HasColumnName("MENU_MODIFIEDON");
        });

        // Configure UserMenuMap entity
        modelBuilder.Entity<UserMenuMap>(entity =>
        {
            entity.ToTable("AIMS_USERMENUMAP");
            entity.HasNoKey();
            entity.Property(e => e.UserRoleId)
                .HasColumnName("USER_ROLEID");
            entity.Property(e => e.MenuId)
                .HasColumnName("USER_MENUID");
            entity.Property(e => e.ModifiedBy)
                .HasColumnName("USER_MODIFIEDBY");
            entity.Property(e => e.ModifiedOn)
                .HasColumnName("USER_MODIFIEDON");
        });

        // Configure SPARSHMenu entity
        modelBuilder.Entity<SPARSHMenu>(entity =>
        {
            entity.ToTable("SPARSHMENU_MASTER");
            entity.HasKey(e => e.MenuId);
            entity.Property(e => e.MenuId)
                .HasColumnName("SPARSHMENU_ID")
                .ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasColumnName("SPARSHMENU_NAME")
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(e => e.PageName)
                .HasColumnName("SPARSHMENU_PAGENAME")
                .HasMaxLength(250)
                .IsRequired();
            entity.Property(e => e.LastModifiedBy)
                .HasColumnName("SPARSHMENU_LASTMODIFIEDBY");
            entity.Property(e => e.LastModifiedOn)
                .HasColumnName("SPARSHMENU_LASTMODIFIEDON");
        });

        // Configure SPARSHMenuAccess entity
        modelBuilder.Entity<SPARSHMenuAccess>(entity =>
        {
            entity.ToTable("SPARSHMENU_ACCESS");
            entity.HasKey(e => e.AccessId);
            entity.Property(e => e.AccessId)
                .HasColumnName("ACCESS_ID")
                .ValueGeneratedNever();
            entity.Property(e => e.UnitId)
                .HasColumnName("ACCESS_UNIT");
            entity.Property(e => e.CalendarId)
                .HasColumnName("ACCESS_CALENDAR");
            entity.Property(e => e.GradeCategory)
                .HasColumnName("ACCESS_GRADECATEGORY")
                .HasMaxLength(3)
                .IsRequired();
            entity.Property(e => e.SPARSHMenuId)
                .HasColumnName("ACCESS_SPARSHMENUID");
        });

        // Create indexes
        modelBuilder.Entity<UserRole>()
            .HasIndex(e => e.EmployeeSystemId)
            .HasDatabaseName("IX_AIMS_USERROLE_EMPSYSID");

        modelBuilder.Entity<Menu>()
            .HasIndex(e => e.ParentMenuId)
            .HasDatabaseName("IX_MENU_MASTER_PARENT");

        modelBuilder.Entity<SPARSHMenuAccess>()
            .HasIndex(e => e.UnitId)
            .HasDatabaseName("IX_SPARSHMENU_ACCESS_UNIT");
    }
}
