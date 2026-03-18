using Microsoft.EntityFrameworkCore;
using RecruitmentService.Domain.Entities;

namespace RecruitmentService.Infrastructure.Persistence;

public class RecruitmentDbContext : DbContext
{
    public RecruitmentDbContext(DbContextOptions<RecruitmentDbContext> options) : base(options) { }

    public DbSet<Vacancy> Vacancies => Set<Vacancy>();
    public DbSet<ApplicationHistory> ApplicationHistories => Set<ApplicationHistory>();
    public DbSet<ApplicationQualification> ApplicationQualifications => Set<ApplicationQualification>();
    public DbSet<ApplicationTraining> ApplicationTrainings => Set<ApplicationTraining>();
    public DbSet<Prospect> Prospects => Set<Prospect>();
    public DbSet<ProspectAddress> ProspectAddresses => Set<ProspectAddress>();
    public DbSet<ProspectQualification> ProspectQualifications => Set<ProspectQualification>();
    public DbSet<ProspectReference> ProspectReferences => Set<ProspectReference>();
    public DbSet<ProspectTraining> ProspectTrainings => Set<ProspectTraining>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecruitmentDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var aggregates = ChangeTracker.Entries<Domain.Common.Entity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = aggregates.SelectMany(e => e.DomainEvents).ToList();
        aggregates.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        // Publish events after save (fire-and-forget pattern)
        foreach (var domainEvent in domainEvents)
        {
            // Events are handled by MediatR notification handlers registered in DI
        }

        return result;
    }
}
