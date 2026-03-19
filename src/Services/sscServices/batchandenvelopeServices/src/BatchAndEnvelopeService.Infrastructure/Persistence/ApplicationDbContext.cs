using Microsoft.EntityFrameworkCore;
using BatchAndEnvelopeService.Domain.Aggregates;
using BatchAndEnvelopeService.Domain.Entities;
using BatchAndEnvelopeService.Domain.Common;
using BatchAndEnvelopeService.Domain.Interfaces;
using MediatR;

namespace BatchAndEnvelopeService.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<BatchAggregate> Batches => Set<BatchAggregate>();
    public DbSet<BatchDetail> BatchDetails => Set<BatchDetail>();
    public DbSet<BatchReceiptDetail> BatchReceiptDetails => Set<BatchReceiptDetail>();
    public DbSet<EnvelopeAggregate> Envelopes => Set<EnvelopeAggregate>();
    public DbSet<EnvelopeDetail> EnvelopeDetails => Set<EnvelopeDetail>();
    public DbSet<EnvelopeReceiptDetail> EnvelopeReceiptDetails => Set<EnvelopeReceiptDetail>();
    public DbSet<ScanLotMaster> ScanLotMasters => Set<ScanLotMaster>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        var entities = ChangeTracker.Entries<Entity<long>>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish((INotification)domainEvent, ct);
    }
}
