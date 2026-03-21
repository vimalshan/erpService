using MediatR;
using Microsoft.EntityFrameworkCore;
using ReceivingService.Domain.Common;
using ReceivingService.Domain.Entities;

namespace ReceivingService.Infrastructure.Data;

public sealed class ReceivingDbContext : DbContext
{
    private readonly IMediator _mediator;

    public ReceivingDbContext(
        DbContextOptions<ReceivingDbContext> options,
        IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Domain.Entities.Receiving>    Receivings     { get; set; } = null!;
    public DbSet<ReceivingLine> ReceivingLines { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReceivingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Persist changes then dispatch accumulated domain events.
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        var entities = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToList();

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        return result;
    }
}

