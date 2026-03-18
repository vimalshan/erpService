using CanteenUnit.Application.Common.Interfaces;
using CanteenUnit.Domain.Common;
using CanteenUnit.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CanteenUnit.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<CanteenUnitMaster> CanteenUnitMasters => Set<CanteenUnitMaster>();
    public DbSet<CanteenMaster> CanteenMasters => Set<CanteenMaster>();
    public DbSet<CanteenMasterCat> CanteenMasterCats => Set<CanteenMasterCat>();
    public DbSet<CanteenMasterGradeCat> CanteenMasterGradeCats => Set<CanteenMasterGradeCat>();
    public DbSet<CanteenUnitAccess> CanteenUnitAccesses => Set<CanteenUnitAccess>();
    public DbSet<GenCounter> GenCounters => Set<GenCounter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync()
    {
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent);
    }
}
