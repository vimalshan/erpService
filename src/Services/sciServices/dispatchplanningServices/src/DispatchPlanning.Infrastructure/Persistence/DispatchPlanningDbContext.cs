using DispatchPlanning.Domain.Aggregates;
using DispatchPlanning.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DispatchPlanning.Infrastructure.Persistence;

public class DispatchPlanningDbContext : DbContext
{
    public DispatchPlanningDbContext(DbContextOptions<DispatchPlanningDbContext> options)
        : base(options) { }

    public DbSet<DispatchPlanAggregate> DispatchPlanHeaders => Set<DispatchPlanAggregate>();
    public DbSet<DispatchPlanMainGroup> DispatchPlanMainGroups => Set<DispatchPlanMainGroup>();
    public DbSet<DispatchPlanSubGroup> DispatchPlanSubGroups => Set<DispatchPlanSubGroup>();
    public DbSet<DispatchPlanBreakupItem> DispatchPlanBreakupItems => Set<DispatchPlanBreakupItem>();
    public DbSet<DispatchPlanItemwise> DispatchPlanItemwises => Set<DispatchPlanItemwise>();
    public DbSet<DispatchPlanSubGroupwise> DispatchPlanSubGroupwises => Set<DispatchPlanSubGroupwise>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DispatchPlanningDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var aggregates = ChangeTracker.Entries<Domain.Common.Entity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = aggregates.SelectMany(a => a.DomainEvents).ToList();
        aggregates.ForEach(a => a.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            // Domain event publishing is handled by the DomainEventDispatcher
            _domainEvents.Add(domainEvent);
        }

        return result;
    }

    private readonly List<Domain.Common.IDomainEvent> _domainEvents = new();
    public IReadOnlyList<Domain.Common.IDomainEvent> GetPendingDomainEvents() => _domainEvents.AsReadOnly();
    public void ClearPendingDomainEvents() => _domainEvents.Clear();
}
