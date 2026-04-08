using MediatR;
using PFTransactionalService.Domain.Common;
using PFTransactionalService.Domain.Interfaces;
using PFTransactionalService.Infrastructure.Persistence.EfCore;

namespace PFTransactionalService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly PFTransactionalDbContext _context;
    private readonly IMediator _mediator;
    private IPFAccumulationRepository? _accumulations;
    private IPFSettlementRepository? _settlements;

    public UnitOfWork(PFTransactionalDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public IPFAccumulationRepository Accumulations =>
        _accumulations ??= new PFAccumulationRepository(_context);

    public IPFSettlementRepository Settlements =>
        _settlements ??= new PFSettlementRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
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
