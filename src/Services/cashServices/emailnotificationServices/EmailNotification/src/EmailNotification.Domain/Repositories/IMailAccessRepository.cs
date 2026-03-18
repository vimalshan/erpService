namespace EmailNotification.Domain.Repositories;

/// <summary>
/// Repository interface for MailAccess entity
/// </summary>
public interface IMailAccessRepository
{
    /// <summary>
    /// Gets a mail access record by its ID
    /// </summary>
    /// <param name="id">The mail access ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The mail access record or null if not found</returns>
    Task<Entities.MailAccess?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets mail access records by email type ID
    /// </summary>
    /// <param name="emailTypeId">The email type ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Enumerable of mail access records for the email type</returns>
    Task<IEnumerable<Entities.MailAccess>> GetByEmailTypeIdAsync(long emailTypeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets mail access records by organization and business unit
    /// </summary>
    /// <param name="emailTypeId">The email type ID</param>
    /// <param name="orgId">The organization ID</param>
    /// <param name="businessId">The business unit ID (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Enumerable of mail access records matching the criteria</returns>
    Task<IEnumerable<Entities.MailAccess>> GetByOrgAndBusinessAsync(
        long emailTypeId,
        long orgId,
        long? businessId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all mail access records
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Enumerable of all mail access records</returns>
    Task<IEnumerable<Entities.MailAccess>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new mail access record
    /// </summary>
    /// <param name="mailAccess">The mail access record to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task AddAsync(Entities.MailAccess mailAccess, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing mail access record
    /// </summary>
    /// <param name="mailAccess">The mail access record to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UpdateAsync(Entities.MailAccess mailAccess, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a mail access record
    /// </summary>
    /// <param name="id">The mail access ID to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
