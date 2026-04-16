using AuditService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuditService.Infrastructure.Data;

public class AuditDomainDbContext : DbContext
{
    public AuditDomainDbContext(DbContextOptions<AuditDomainDbContext> options) : base(options) { }

    public DbSet<Audit> Audits => Set<Audit>();
    public DbSet<AuditType> AuditTypes => Set<AuditType>();
    public DbSet<AuditSite> AuditSites => Set<AuditSite>();
    public DbSet<AuditServiceEntity> AuditServices => Set<AuditServiceEntity>();
    public DbSet<AuditTeamMember> AuditTeamMembers => Set<AuditTeamMember>();
    public DbSet<AuditSiteAudit> AuditSiteAudits => Set<AuditSiteAudit>();
    public DbSet<AuditSiteRepresentative> AuditSiteRepresentatives => Set<AuditSiteRepresentative>();
    public DbSet<AuditSiteService> AuditSiteServices => Set<AuditSiteService>();
    public DbSet<SiteInfo> Sites => Set<SiteInfo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audit>(e =>
        {
            e.ToTable("Audits");
            e.HasKey(x => x.AuditId);
            e.Property(x => x.AuditId).HasColumnName("auditId");
            e.Property(x => x.Sites).HasColumnName("sites");
            e.Property(x => x.Services).HasColumnName("services");
            e.Property(x => x.CompanyId).HasColumnName("companyId");
            e.Property(x => x.Status).HasColumnName("status").HasMaxLength(50);
            e.Property(x => x.StartDate).HasColumnName("startDate");
            e.Property(x => x.EndDate).HasColumnName("endDate");
            e.Property(x => x.LeadAuditor).HasColumnName("leadAuditor").HasMaxLength(100);
            e.Property(x => x.Type).HasColumnName("type").HasMaxLength(50);
            e.Ignore(x => x.DomainEvents);
            e.HasMany(x => x.AuditSites).WithOne(x => x.Audit).HasForeignKey(x => x.AuditId);
            e.HasMany(x => x.AuditServices).WithOne(x => x.Audit).HasForeignKey(x => x.AuditId);
            e.HasMany(x => x.AuditTeamMembers).WithOne(x => x.Audit).HasForeignKey(x => x.AuditId);
        });

        modelBuilder.Entity<AuditType>(e =>
        {
            e.ToTable("AuditTypes");
            e.HasKey(x => x.AuditTypeId);
        });

        modelBuilder.Entity<AuditSite>(e =>
        {
            e.ToTable("AuditSites");
            e.HasKey(x => x.AuditSiteId);
        });

        modelBuilder.Entity<AuditServiceEntity>(e =>
        {
            e.ToTable("AuditServices");
            e.HasKey(x => x.AuditServiceId);
        });

        modelBuilder.Entity<AuditTeamMember>(e =>
        {
            e.ToTable("AuditTeamMembers");
            e.HasKey(x => x.AuditTeamMemberId);
        });

        modelBuilder.Entity<AuditSiteAudit>(e =>
        {
            e.ToTable("AuditSiteAudits");
            e.HasKey(x => x.AuditSiteAuditId);
            e.HasOne(x => x.AuditTypeNavigation).WithMany().HasForeignKey(x => x.AuditTypeId);
            e.HasMany(x => x.Representatives).WithOne(x => x.AuditSiteAudit).HasForeignKey(x => x.AuditSiteAuditId);
            e.HasMany(x => x.SiteServices).WithOne(x => x.AuditSiteAudit).HasForeignKey(x => x.AuditSiteAuditId);
        });

        modelBuilder.Entity<AuditSiteRepresentative>(e =>
        {
            e.ToTable("AuditSiteRepresentatives");
            e.HasKey(x => x.AuditSiteRepresentativeId);
        });

        modelBuilder.Entity<AuditSiteService>(e =>
        {
            e.ToTable("AuditSiteServices");
            e.HasKey(x => x.AuditSiteServiceId);
            e.Property(x => x.Cost).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<SiteInfo>(e =>
        {
            e.ToTable("Sites");
            e.HasKey(x => x.SiteId);
        });
    }
}
