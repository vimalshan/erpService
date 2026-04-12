using MediatR;
using travelTransactionService.Domain.Common;
using travelTransactionService.Domain.Interfaces;
using travelTransactionService.Infrastructure.Data;

namespace travelTransactionService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TransactionDbContext _context;
    private readonly IMediator _mediator;

    public IVendorMasterRepository Vendors { get; }
    public ITaxMasterRepository TaxMasters { get; }
    public IJaiInterfaceLineRepository JaiInterfaceLines { get; }

    public UnitOfWork(
        TransactionDbContext context,
        IMediator mediator,
        IVendorMasterRepository vendors,
        ITaxMasterRepository taxMasters,
        IJaiInterfaceLineRepository jaiInterfaceLines)
    {
        _context = context;
        _mediator = mediator;
        Vendors = vendors;
        TaxMasters = taxMasters;
        JaiInterfaceLines = jaiInterfaceLines;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = _context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        var result = await _context.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
