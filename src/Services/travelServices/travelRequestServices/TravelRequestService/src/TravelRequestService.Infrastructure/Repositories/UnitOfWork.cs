using MediatR;
using TravelRequestService.Domain.Common;
using TravelRequestService.Domain.Interfaces;
using TravelRequestService.Infrastructure.Data;

namespace TravelRequestService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TravelDbContext _context;
    private readonly IMediator _mediator;

    public ITravelRequestRepository TravelRequests { get; }
    public ITravelAdvanceRepository TravelAdvances { get; }

    public UnitOfWork(
        TravelDbContext context,
        IMediator mediator,
        ITravelRequestRepository travelRequests,
        ITravelAdvanceRepository travelAdvances)
    {
        _context = context;
        _mediator = mediator;
        TravelRequests = travelRequests;
        TravelAdvances = travelAdvances;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
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
