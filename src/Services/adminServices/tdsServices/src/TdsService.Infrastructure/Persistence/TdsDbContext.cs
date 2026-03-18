using Microsoft.EntityFrameworkCore;
using TdsService.Application.Common.Interfaces;
using TdsService.Domain.Common;
using TdsService.Domain.Entities;
using MediatR;

namespace TdsService.Infrastructure.Persistence;

public sealed class TdsDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;

    public TdsDbContext(DbContextOptions<TdsDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<TdsVendor> TdsVendors => Set<TdsVendor>();
    public DbSet<TdsFile> TdsFiles => Set<TdsFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TdsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect and dispatch domain events before persisting
        var aggregates = ChangeTracker
            .Entries<AggregateRoot<long>>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        aggregates.ForEach(a => a.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        return result;
    }
}
