using MediatR;
using Microsoft.EntityFrameworkCore;
using StipendService.Domain.Common;
using StipendService.Domain.Entities;

namespace StipendService.Infrastructure.Persistence;

public class StipendDbContext : DbContext
{
    private readonly IMediator _mediator;

    public StipendDbContext(DbContextOptions<StipendDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<StipendMaster> StipendMasters => Set<StipendMaster>();
    public DbSet<StipendDisbursement> StipendDisbursements => Set<StipendDisbursement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StipendDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEventsAsync(cancellationToken);
        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        foreach (var entity in entities)
        {
            foreach (var domainEvent in entity.DomainEvents)
                await _mediator.Publish(domainEvent, cancellationToken);
            entity.ClearDomainEvents();
        }
    }
}
