using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data;

/// <summary>
/// EF Core DbContext for User Service
/// </summary>
public class UserServiceDbContext : DbContext
{
    public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserRoleMapping> UserRoleMappings => Set<UserRoleMapping>();
    public DbSet<UserOrganizationMapping> UserOrganizationMappings => Set<UserOrganizationMapping>();
    public DbSet<UserLocationMapping> UserLocationMappings => Set<UserLocationMapping>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("USER_MAST");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("USER_ID")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasColumnName("USER_NAME")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .HasColumnName("USER_PASSWORD")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.EmailId)
                .HasColumnName("USER_EMAILID")
                .HasMaxLength(50);

            builder.Property(x => x.SparchUserId)
                .HasColumnName("USER_SPARSHUSERID")
                .HasMaxLength(50);

            builder.Property(x => x.HrEmpSysId)
                .HasColumnName("USER_HREMPSYSID");

            builder.Property(x => x.EffectiveDate)
                .HasColumnName("USER_EFFECTIVE_DATE")
                .HasColumnType("datetime2(3)");

            builder.Property(x => x.ClosureDate)
                .HasColumnName("USER_CLOSURE_DATE")
                .HasColumnType("datetime2(3)");

            builder.Property(x => x.EnteredBy)
                .HasColumnName("USER_ENTEREDBY");

            builder.Property(x => x.CreatedDate)
                .HasColumnName("CREATED_DATE")
                .HasColumnType("datetime2(3)")
                .IsRequired();

            builder.Property(x => x.ModifiedDate)
                .HasColumnName("MODIFIED_DATE")
                .HasColumnType("datetime2(3)");

            builder.Property(x => x.IsActive)
                .HasColumnName("IS_ACTIVE")
                .HasDefaultValue(true);

            builder.HasMany(x => x.RoleMappings)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.OrganizationMappings)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.LocationMappings)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure UserRoleMapping entity
        modelBuilder.Entity<UserRoleMapping>(builder =>
        {
            builder.ToTable("USER_ROLEMAP");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("ROLE_MAPID")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.UserId)
                .HasColumnName("ROLE_USERID")
                .IsRequired();

            builder.Property(x => x.RoleId)
                .HasColumnName("ROLE_ID")
                .IsRequired();

            builder.Property(x => x.IsDefault)
                .HasColumnName("ROLE_DEFFLAG")
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedDate)
                .HasColumnName("ROLE_CREATEDON")
                .HasColumnType("datetime2(3)");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("ROLE_CREATEDBY");
        });

        // Configure UserOrganizationMapping entity
        modelBuilder.Entity<UserOrganizationMapping>(builder =>
        {
            builder.ToTable("USER_ORGMAP");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("ORG_MAPID")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.UserId)
                .HasColumnName("ORG_USERID")
                .IsRequired();

            builder.Property(x => x.BusinessUnitId)
                .HasColumnName("ORG_BUID")
                .HasMaxLength(25)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .HasColumnName("ORG_CREATEDON")
                .HasColumnType("datetime2(3)");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("ORG_CREATEDBY");
        });

        // Configure UserLocationMapping entity
        modelBuilder.Entity<UserLocationMapping>(builder =>
        {
            builder.ToTable("USER_LOCATIONMAP");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("LOC_MAPID")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.UserId)
                .HasColumnName("LOC_USERID")
                .IsRequired();

            builder.Property(x => x.LocationId)
                .HasColumnName("LOC_ID")
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .HasColumnName("LOC_CREATEDON")
                .HasColumnType("datetime2(3)");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("LOC_CREATEDBY");
        });
    }
}
