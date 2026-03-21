using EnergyService.Domain.Common;
using EnergyService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnergyService.Infrastructure.Persistence;

public class EnergyDbContext : DbContext
{
    private readonly IMediator _mediator;

    public EnergyDbContext(DbContextOptions<EnergyDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<EcProcess> EcProcesses => Set<EcProcess>();
    public DbSet<EcProcessAccess> EcProcessAccesses => Set<EcProcessAccess>();
    public DbSet<EcProcessMailId> EcProcessMailIds => Set<EcProcessMailId>();
    public DbSet<EcReading> EcReadings => Set<EcReading>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EnergyDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
