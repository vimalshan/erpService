using EmployeeTransactionsService.Application.DTOs;
using EmployeeTransactionsService.Domain.Common;

namespace EmployeeTransactionsService.Application.Contracts;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default);
}

public interface IBlobStorageService
{
    Task<string> UploadAsync(string blobName, byte[] content, string contentType, CancellationToken cancellationToken = default);
}

public interface IJwtTokenService
{
    string GenerateToken(string username, string[] roles);
}

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}

public interface ITransactionReadService
{
    Task<IReadOnlyList<TransactionTimelineItemDto>> GetEmployeeTimelineAsync(decimal employeeId, CancellationToken cancellationToken);
}