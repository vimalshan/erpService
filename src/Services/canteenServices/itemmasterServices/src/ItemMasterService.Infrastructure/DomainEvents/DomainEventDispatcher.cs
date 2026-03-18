using MediatR;
using Microsoft.Extensions.Logging;
using ItemMasterService.Domain.Common;
using ItemMasterService.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;

namespace ItemMasterService.Infrastructure.DomainEvents;

public class DomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(IMediator mediator, ILogger<DomainEventDispatcher> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task DispatchEventsAsync(ItemMasterDbContext db, CancellationToken ct = default)
    {
        var aggregates = db.ChangeTracker.Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var events = aggregates.SelectMany(a => a.DomainEvents).ToList();
        aggregates.ForEach(a => a.ClearDomainEvents());

        foreach (var domainEvent in events)
        {
            _logger.LogInformation("[DomainEvent] Dispatching {EventName}", domainEvent.GetType().Name);
            await _mediator.Publish(domainEvent, ct);
        }
    }
}
