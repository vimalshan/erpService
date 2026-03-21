using Microsoft.EntityFrameworkCore;
using AdminService.Domain.Entities;
using AdminService.Domain.Common;
using MediatR;

namespace AdminService.Infrastructure.Data;

public class AdminDbContext : DbContext
{
    private readonly IMediator? _mediator;

    public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }

    public AdminDbContext(DbContextOptions<AdminDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<AdminMaster> AdminMasters => Set<AdminMaster>();
    public DbSet<AdminUserMap> AdminUserMaps => Set<AdminUserMap>();
    public DbSet<AdminFinUserMap> AdminFinUserMaps => Set<AdminFinUserMap>();
    public DbSet<AdminAccessRights> AdminAccessRights => Set<AdminAccessRights>();
    public DbSet<AdminAccessRightsLog> AdminAccessRightsLogs => Set<AdminAccessRightsLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ADMIN_MASTER
        modelBuilder.Entity<AdminMaster>(e =>
        {
            e.ToTable("ADMIN_MASTER");
            e.HasKey(x => x.AdminId);
            e.Property(x => x.AdminId).HasColumnName("ADMIN_ID").HasMaxLength(255);
            e.Property(x => x.AdminName).HasColumnName("ADMIN_NAME").HasMaxLength(255);
            e.Property(x => x.AdminPic).HasColumnName("ADMIN_PIC").HasMaxLength(255);
            e.Property(x => x.AdminUnitId).HasColumnName("ADMIN_UNITID").HasMaxLength(255);
            e.Property(x => x.AdminUnitHeadSysId).HasColumnName("ADMIN_UNITHEADSYSID").HasMaxLength(255);
            e.Property(x => x.AdminLocStatus).HasColumnName("ADMIN_LOCSTATUS").HasMaxLength(1);
            e.Ignore(x => x.CreatedOn);
            e.Ignore(x => x.CreatedBy);
            e.Ignore(x => x.ModifiedOn);
            e.Ignore(x => x.ModifiedBy);
        });

        // ADMIN_USERMAP
        modelBuilder.Entity<AdminUserMap>(e =>
        {
            e.ToTable("ADMIN_USERMAP");
            e.HasKey(x => x.AdminMapId);
            e.Property(x => x.AdminMapId).HasColumnName("ADMIN_MAPID").HasMaxLength(255);
            e.Property(x => x.AdminBookType).HasColumnName("ADMIN_BOOKTYPE").HasMaxLength(255);
            e.Property(x => x.AdminMode).HasColumnName("ADMIN_MODE").HasMaxLength(255);
            e.Property(x => x.AdminEmpSysId).HasColumnName("ADMIN_EMPSYSID").HasMaxLength(255);
            e.Property(x => x.AdminId).HasColumnName("ADMIN_ID").HasMaxLength(255);
            e.Property(x => x.AdminLastModifiedBy).HasColumnName("ADMIN_LASTMODIFIEDBY").HasMaxLength(255);
            e.Property(x => x.AdminLastModifiedOn).HasColumnName("ADMIN_LASTMODIFIEDON").HasPrecision(3);
            e.HasOne(x => x.Admin).WithMany(a => a.UserMaps).HasForeignKey(x => x.AdminId);
            e.Ignore(x => x.CreatedOn);
            e.Ignore(x => x.CreatedBy);
            e.Ignore(x => x.ModifiedOn);
            e.Ignore(x => x.ModifiedBy);
        });

        // ADMIN_FINUSERMAP
        modelBuilder.Entity<AdminFinUserMap>(e =>
        {
            e.ToTable("ADMIN_FINUSERMAP");
            e.HasKey(x => x.FinanceMapId);
            e.Property(x => x.FinanceMapId).HasColumnName("FINANCE_MAPID").HasMaxLength(255);
            e.Property(x => x.FinancePayUnitId).HasColumnName("FINANCE_PAYUNITID").HasMaxLength(255);
            e.Property(x => x.FinanceEmpSysId).HasColumnName("FINANCE_EMPSYSID").HasMaxLength(255);
            e.Property(x => x.FinanceLastModifiedBy).HasColumnName("FINANCE_LASTMODIFIEDBY").HasMaxLength(255);
            e.Property(x => x.FinanceLastModifiedOn).HasColumnName("FINANCE_LASTMODIFIEDON").HasPrecision(3);
            e.Ignore(x => x.CreatedOn);
            e.Ignore(x => x.CreatedBy);
            e.Ignore(x => x.ModifiedOn);
            e.Ignore(x => x.ModifiedBy);
        });

        // ADMIN_ACCESSRIGHTS
        modelBuilder.Entity<AdminAccessRights>(e =>
        {
            e.ToTable("ADMIN_ACCESSRIGHTS");
            e.HasKey(x => x.AdminRightsId);
            e.Property(x => x.AdminRightsId).HasColumnName("ADMIN_RIGHTSID").HasMaxLength(255);
            e.Property(x => x.AdminLocationId).HasColumnName("ADMIN_LOCATIONID").HasMaxLength(255);
            e.Property(x => x.AdminRightsFor).HasColumnName("ADMIN_RIGHTSFOR").HasMaxLength(255);
            e.Property(x => x.AdminRightsType).HasColumnName("ADMIN_RIGHTSTYPE").HasMaxLength(255);
            e.Property(x => x.AdminUserId).HasColumnName("ADMIN_USERID").HasMaxLength(255);
            e.Property(x => x.AdminAlertId).HasColumnName("ADMIN_ALERTID").HasMaxLength(255);
            e.Property(x => x.AdminContactNo).HasColumnName("ADMIN_CONTACTNO").HasMaxLength(255);
            e.Property(x => x.AdminContactDes).HasColumnName("ADMIN_CONTACTDES").HasMaxLength(255);
            e.Property(x => x.AdminEntOn).HasColumnName("ADMIN_ENTON").HasPrecision(3);
            e.Property(x => x.AdminEntBy).HasColumnName("ADMIN_ENTBY").HasMaxLength(255);
            e.HasOne(x => x.Admin).WithMany(a => a.AccessRights).HasForeignKey(x => x.AdminLocationId);
            e.Ignore(x => x.CreatedOn);
            e.Ignore(x => x.CreatedBy);
            e.Ignore(x => x.ModifiedOn);
            e.Ignore(x => x.ModifiedBy);
        });

        // ADMIN_ACCESSRIGHTSLOG
        modelBuilder.Entity<AdminAccessRightsLog>(e =>
        {
            e.ToTable("ADMIN_ACCESSRIGHTSLOG");
            e.HasKey(x => new { x.AdminRightsId, x.AdminLogId });
            e.Property(x => x.AdminLogId).HasColumnName("ADMIN_LOGID").HasMaxLength(255);
            e.Property(x => x.AdminRightsId).HasColumnName("ADMIN_RIGHTSID").HasMaxLength(255);
            e.Property(x => x.AdminLocationId).HasColumnName("ADMIN_LOCATIONID").HasMaxLength(255);
            e.Property(x => x.AdminRightsFor).HasColumnName("ADMIN_RIGHTSFOR").HasMaxLength(255);
            e.Property(x => x.AdminRightsType).HasColumnName("ADMIN_RIGHTSTYPE").HasMaxLength(255);
            e.Property(x => x.AdminUserId).HasColumnName("ADMIN_USERID").HasMaxLength(255);
            e.Property(x => x.AdminAlertId).HasColumnName("ADMIN_ALERTID").HasMaxLength(255);
            e.Property(x => x.AdminContactNo).HasColumnName("ADMIN_CONTACTNO").HasMaxLength(255);
            e.Property(x => x.AdminContactDes).HasColumnName("ADMIN_CONTACTDES").HasMaxLength(255);
            e.Property(x => x.AdminEntOn).HasColumnName("ADMIN_ENTON").HasPrecision(3);
            e.Property(x => x.AdminEntBy).HasColumnName("ADMIN_ENTBY").HasMaxLength(255);
            e.HasOne(x => x.AccessRights).WithMany(a => a.AccessRightsLogs).HasForeignKey(x => x.AdminRightsId);
            e.Ignore(x => x.CreatedOn);
            e.Ignore(x => x.CreatedBy);
            e.Ignore(x => x.ModifiedOn);
            e.Ignore(x => x.ModifiedBy);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events after save
        var result = await base.SaveChangesAsync(cancellationToken);

        if (_mediator is not null)
        {
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var domainEvents = entitiesWithEvents.SelectMany(e => e.DomainEvents).ToList();
            entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
