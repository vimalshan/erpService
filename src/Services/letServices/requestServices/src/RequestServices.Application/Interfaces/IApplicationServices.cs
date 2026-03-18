using RequestServices.Domain.Common;

namespace RequestServices.Application.Interfaces;

/// <summary>Dispatches domain events raised by aggregates to registered MediatR handlers.</summary>
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<DomainEvent> events, CancellationToken ct = default);
}

/// <summary>Blob storage abstraction for storing stationery item images.</summary>
public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default);
    Task<bool> ExistsAsync(string containerName, string blobName, CancellationToken ct = default);
    string GetBlobUrl(string containerName, string blobName);
}

/// <summary>Message bus abstraction for publishing integration events.</summary>
public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : class;
}
