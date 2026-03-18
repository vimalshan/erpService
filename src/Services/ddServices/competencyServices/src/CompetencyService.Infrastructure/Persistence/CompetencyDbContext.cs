using Microsoft.EntityFrameworkCore;
using CompetencyService.Domain.Entities;
using CompetencyService.Domain.Common;
using MediatR;

namespace CompetencyService.Infrastructure.Persistence;

public class CompetencyDbContext(DbContextOptions<CompetencyDbContext> options, IMediator mediator)
    : DbContext(options)
{
    public DbSet<CompetencyMaster> CompetencyMasters => Set<CompetencyMaster>();
    public DbSet<CompetencyIndicator> CompetencyIndicators => Set<CompetencyIndicator>();
    public DbSet<CompetencyRatingScale> CompetencyRatingScales => Set<CompetencyRatingScale>();
    public DbSet<BandCoreCompetency> BandCoreCompetencies => Set<BandCoreCompetency>();
    public DbSet<EmpSpecificCompetency> EmpSpecificCompetencies => Set<EmpSpecificCompetency>();
    public DbSet<RoleSpecific> RoleSpecifics => Set<RoleSpecific>();
    public DbSet<VtcCompetency> VtcCompetencies => Set<VtcCompetency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CompetencyDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEventsAsync(cancellationToken);
        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var events = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in events)
        {
            // Wrap and publish via MediatR notification adapter
            await mediator.Publish(WrapDomainEvent(domainEvent), ct);
        }
    }

    private static object WrapDomainEvent(IDomainEvent domainEvent) =>
        domainEvent switch
        {
            Domain.Events.CompetencyCreatedEvent e =>
                new Application.EventHandlers.CompetencyCreatedDomainEventNotification(e),
            Domain.Events.CompetencyUpdatedEvent e =>
                new Application.EventHandlers.CompetencyUpdatedDomainEventNotification(e),
            Domain.Events.CompetencyClosedEvent e =>
                new Application.EventHandlers.CompetencyClosedDomainEventNotification(e),
            Domain.Events.EmpCompetencyAssignedEvent e =>
                new Application.EventHandlers.EmpCompetencyAssignedDomainEventNotification(e),
            _ => throw new InvalidOperationException($"Unknown domain event: {domainEvent.GetType().Name}")
        };
}
