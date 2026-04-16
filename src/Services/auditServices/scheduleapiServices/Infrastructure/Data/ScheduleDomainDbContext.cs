using ScheduleService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ScheduleService.Infrastructure.Data;

public class ScheduleDomainDbContext : DbContext
{
    public ScheduleDomainDbContext(DbContextOptions<ScheduleDomainDbContext> options) : base(options) { }

    public DbSet<AuditSiteAudit> AuditSiteAudits => Set<AuditSiteAudit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditSiteAudit>(e =>
        {
            e.ToTable("AuditSiteAudits"); e.HasKey(x => x.AuditSiteAuditId);
            e.Property(x => x.AuditNumber).HasMaxLength(50).IsRequired();
            e.Property(x => x.Status).HasMaxLength(50).HasDefaultValue("scheduled");
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.ModifiedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.Notes).HasMaxLength(1000);
            e.Property(x => x.ReportPath).HasMaxLength(500);
            e.Property(x => x.CertificateIssued).HasDefaultValue(false);
            e.Property(x => x.CertificateNumber).HasMaxLength(100);
            e.HasIndex(x => x.AuditNumber).IsUnique();
            e.HasIndex(x => x.AuditId); e.HasIndex(x => x.SiteId);
            e.HasIndex(x => x.Status); e.HasIndex(x => x.ScheduledDate);
            e.HasIndex(x => x.LeadAuditorId);
            e.Ignore(x => x.DomainEvents);
        });
    }
}
