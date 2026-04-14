using MediatR;
using Microsoft.EntityFrameworkCore;
using WarehouseStructure.Domain.Common;
using WarehouseStructure.Domain.Entities;

namespace WarehouseStructure.Infrastructure.Persistence;

public class WarehouseStructureDbContext : DbContext
{
    private readonly IMediator _mediator;

    public WarehouseStructureDbContext(DbContextOptions<WarehouseStructureDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Zone> Zones => Set<Zone>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WarehouseStructureDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect events from entities being deleted (they'll be detached after save)
        var deletedRoots = ChangeTracker.Entries<AggregateRoot>()
            .Where(e => e.State == EntityState.Deleted && e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var preDeleteEvents = deletedRoots
            .SelectMany(ar => ar.DomainEvents)
            .ToList();

        foreach (var ar in deletedRoots)
            ar.ClearDomainEvents();

        var result = await base.SaveChangesAsync(cancellationToken);

        // Collect events from remaining tracked entities (IDs now populated for new entities)
        var postSaveRoots = ChangeTracker.Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var postSaveEvents = postSaveRoots
            .SelectMany(ar => ar.DomainEvents)
            .ToList();

        foreach (var ar in postSaveRoots)
            ar.ClearDomainEvents();

        // Dispatch all events: pre-delete + post-save
        foreach (var domainEvent in preDeleteEvents.Concat(postSaveEvents))
            await _mediator.Publish(domainEvent, cancellationToken);

        return result;
    }
}
