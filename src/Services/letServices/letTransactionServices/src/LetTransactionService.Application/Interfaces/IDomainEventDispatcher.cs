using LetTransactionService.Domain.Common;

namespace LetTransactionService.Application.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<DomainEvent> events, CancellationToken ct = default);
}
