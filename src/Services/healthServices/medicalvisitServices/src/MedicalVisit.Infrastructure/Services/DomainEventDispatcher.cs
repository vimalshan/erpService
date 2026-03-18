using MediatR;
using MedicalVisit.Domain.Common;
using MedicalVisit.Infrastructure.Persistence;

namespace MedicalVisit.Infrastructure.Services;

public class DomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly MedicalVisitDbContext _context;

    public DomainEventDispatcher(IMediator mediator, MedicalVisitDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task DispatchEventsAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = _context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
