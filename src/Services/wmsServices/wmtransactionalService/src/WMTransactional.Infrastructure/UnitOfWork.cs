using MediatR;
using WMTransactional.Domain.Common;
using WMTransactional.Domain.Interfaces;
using WMTransactional.Infrastructure.Persistence;
using WMTransactional.Infrastructure.Repositories;

namespace WMTransactional.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly WMTransactionalDbContext _context;
    private readonly IMediator _mediator;
    private IPurchaseOrderRepository? _purchaseOrders;
    private IReceivingRepository? _receivings;
    private ISalesOrderRepository? _salesOrders;
    private IShipmentRepository? _shipments;

    public UnitOfWork(WMTransactionalDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public IPurchaseOrderRepository PurchaseOrders =>
        _purchaseOrders ??= new PurchaseOrderRepository(_context);

    public IReceivingRepository Receivings =>
        _receivings ??= new ReceivingRepository(_context);

    public ISalesOrderRepository SalesOrders =>
        _salesOrders ??= new SalesOrderRepository(_context);

    public IShipmentRepository Shipments =>
        _shipments ??= new ShipmentRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Collect domain events from tracked entities before saving
        var entities = _context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        // Clear domain events before publishing to avoid re-entrancy
        entities.ForEach(e => e.ClearDomainEvents());

        // Save changes first
        var result = await _context.SaveChangesAsync(ct);

        // Dispatch domain events after successful save
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, ct);
        }

        return result;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
