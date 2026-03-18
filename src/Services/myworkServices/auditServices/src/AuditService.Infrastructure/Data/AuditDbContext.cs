using AuditService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuditService.Infrastructure.Data;

public class AuditDbContext : DbContext
{
    public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options) { }

    public DbSet<AuditMaster> AuditMasters => Set<AuditMaster>();
    public DbSet<AuditObservation> AuditObservations => Set<AuditObservation>();
    public DbSet<AuditGoodPractice> AuditGoodPractices => Set<AuditGoodPractice>();
    public DbSet<AuditGoodPracticeRating> AuditGoodPracticeRatings => Set<AuditGoodPracticeRating>();
    public DbSet<AuditObservationApp> AuditObservationApps => Set<AuditObservationApp>();
    public DbSet<AuditProcessMaster> AuditProcessMasters => Set<AuditProcessMaster>();
    public DbSet<AuditUserAccess> AuditUserAccesses => Set<AuditUserAccess>();
    public DbSet<AuditUserMaster> AuditUserMasters => Set<AuditUserMaster>();
    public DbSet<AuditYearMaster> AuditYearMasters => Set<AuditYearMaster>();
    public DbSet<IaHtmlEmail> IaHtmlEmails => Set<IaHtmlEmail>();
    public DbSet<IaEscalationMail> IaEscalationMails => Set<IaEscalationMail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuditDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
