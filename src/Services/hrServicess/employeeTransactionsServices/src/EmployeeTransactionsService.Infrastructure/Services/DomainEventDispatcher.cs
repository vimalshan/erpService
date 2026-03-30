using EmployeeTransactionsService.Application.Contracts;
using EmployeeTransactionsService.Domain.Common;
using MediatR;

namespace EmployeeTransactionsService.Infrastructure.Services;

public sealed class DomainEventDispatcher(IPublisher publisher) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
            await publisher.Publish(new DomainEventNotification(domainEvent), cancellationToken);
    }
}

public sealed record DomainEventNotification(IDomainEvent DomainEvent) : INotification;