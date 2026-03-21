using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskServices.Domain.Common;
using TaskServices.Infrastructure.Persistence;

namespace TaskServices.Infrastructure.Services;

public class DomainEventDispatcher
{
    private readonly TaskDbContext _context;
    private readonly IMediator _mediator;

    public DomainEventDispatcher(TaskDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task DispatchEventsAsync(CancellationToken cancellationToken = default)
    {
        var entities = _context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
