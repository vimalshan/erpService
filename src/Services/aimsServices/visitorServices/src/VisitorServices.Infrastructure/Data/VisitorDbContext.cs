using Microsoft.EntityFrameworkCore;
using VisitorServices.Domain.Aggregates;
using VisitorServices.Domain.Common;
using VisitorServices.Domain.Entities;

namespace VisitorServices.Infrastructure.Data;

public class VisitorDbContext(DbContextOptions<VisitorDbContext> options) : DbContext(options)
{
    public DbSet<VisitorAggregate> Visitors => Set<VisitorAggregate>();
    public DbSet<VisitorItem> VisitorItems => Set<VisitorItem>();
    public DbSet<VisitorApprovalRequest> ApprovalRequests => Set<VisitorApprovalRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VisitorDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var aggregates = ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        aggregates.ForEach(a => a.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        // Note: domain event dispatching delegated to DomainEventDispatcher service
        if (domainEvents.Count != 0)
            DomainEventsBeforeSave = domainEvents;

        return result;
    }

    // Exposes events collected during this save cycle for the dispatcher
    public List<IDomainEvent> DomainEventsBeforeSave { get; private set; } = [];
}
