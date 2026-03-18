namespace EmailNotification.Domain.Repositories;

/// <summary>
/// Repository interface for EmailType aggregate
/// </summary>
public interface IEmailTypeRepository
{
    /// <summary>
    /// Gets an email type by its ID
    /// </summary>
    /// <param name="id">The email type ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The email type aggregate or null if not found</returns>
    Task<Aggregates.EmailTypeAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all email types
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Enumerable of all email types</returns>
    Task<IEnumerable<Aggregates.EmailTypeAggregate>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets email types by type (Daily or Event)
    /// </summary>
    /// <param name="emailType">The email type</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Enumerable of email types matching the type</returns>
    Task<IEnumerable<Aggregates.EmailTypeAggregate>> GetByTypeAsync(
        ValueObjects.EmailTypeEnum emailType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new email type
    /// </summary>
    /// <param name="emailType">The email type to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task AddAsync(Aggregates.EmailTypeAggregate emailType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing email type
    /// </summary>
    /// <param name="emailType">The email type to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UpdateAsync(Aggregates.EmailTypeAggregate emailType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an email type
    /// </summary>
    /// <param name="id">The email type ID to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
