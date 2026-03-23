using AttendanceService.Domain.Common;
using AttendanceService.Domain.Interfaces;
using MediatR;

namespace AttendanceService.Infrastructure.Services;

public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default)
    {
        foreach (var domainEvent in events)
            await mediator.Publish(domainEvent, ct);
    }
}
