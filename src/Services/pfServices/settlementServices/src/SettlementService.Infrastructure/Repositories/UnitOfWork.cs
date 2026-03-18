using MediatR;
using SettlementService.Domain.Common;
using SettlementService.Domain.Interfaces;
using SettlementService.Infrastructure.Persistence.EfCore;

namespace SettlementService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly SettlementDbContext _context;
    private readonly IMediator _mediator;
    private ISettlementRepository? _settlements;

    public UnitOfWork(SettlementDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public ISettlementRepository Settlements =>
        _settlements ??= new SettlementRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var domainEntities = _context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities.SelectMany(e => e.DomainEvents).ToList();

        foreach (var entity in domainEntities)
            entity.ClearDomainEvents();

        var result = await _context.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        return result;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
