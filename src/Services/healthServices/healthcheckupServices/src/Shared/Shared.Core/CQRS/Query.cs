namespace Shared.Core.CQRS;

/// <summary>
/// Base class for all queries in CQRS pattern
/// Queries represent a request to retrieve data without side effects
/// </summary>
public abstract record Query<TResponse>
{
    public Guid QueryId { get; init; } = Guid.NewGuid();
    public DateTime IssuedAt { get; init; } = DateTime.UtcNow;
}

/// <summary>
/// Handler interface for queries
/// </summary>
public interface IQueryHandler<in TQuery, TResponse> where TQuery : Query<TResponse>
{
    Task<TResponse> ExecuteAsync(TQuery query, CancellationToken cancellationToken = default);
}
