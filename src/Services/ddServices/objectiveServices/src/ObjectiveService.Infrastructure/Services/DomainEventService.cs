using MediatR;
using ObjectiveService.Application.Interfaces;

namespace ObjectiveService.Infrastructure.Services;

public class DomainEventService : IDomainEventService
{
    private readonly IMediator _mediator;

    public DomainEventService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : INotification
    {
        await _mediator.Publish(domainEvent, cancellationToken);
    }
}
