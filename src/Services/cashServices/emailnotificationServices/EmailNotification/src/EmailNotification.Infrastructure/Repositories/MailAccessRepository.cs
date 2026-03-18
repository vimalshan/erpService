using Microsoft.EntityFrameworkCore;
using EmailNotification.Domain.Entities;
using EmailNotification.Domain.Repositories;

namespace EmailNotification.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for MailAccess entity
/// </summary>
public class MailAccessRepository : IMailAccessRepository
{
    private readonly Data.EmailNotificationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the MailAccessRepository class
    /// </summary>
    /// <param name="context">The DbContext</param>
    public MailAccessRepository(Data.EmailNotificationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets a mail access record by its ID
    /// </summary>
    public async Task<MailAccess?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.MailAccesses
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <summary>
    /// Gets mail access records by email type ID
    /// </summary>
    public async Task<IEnumerable<MailAccess>> GetByEmailTypeIdAsync(long emailTypeId, CancellationToken cancellationToken = default)
    {
        return await _context.MailAccesses
            .Where(x => x.MailTypeId == emailTypeId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets mail access records by organization and business unit
    /// </summary>
    public async Task<IEnumerable<MailAccess>> GetByOrgAndBusinessAsync(
        long emailTypeId,
        long orgId,
        long? businessId = null,
        CancellationToken cancellationToken = default)
    {
        return await _context.MailAccesses
            .Where(x => x.MailTypeId == emailTypeId &&
                   (x.MailOrgId == null || x.MailOrgId == 0 || x.MailOrgId == orgId) &&
                   (businessId == null || x.MailBusinessId == null || x.MailBusinessId == 0 || x.MailBusinessId == businessId))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets all mail access records
    /// </summary>
    public async Task<IEnumerable<MailAccess>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MailAccesses.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Adds a new mail access record
    /// </summary>
    public async Task AddAsync(MailAccess mailAccess, CancellationToken cancellationToken = default)
    {
        _context.MailAccesses.Add(mailAccess);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Updates an existing mail access record
    /// </summary>
    public async Task UpdateAsync(MailAccess mailAccess, CancellationToken cancellationToken = default)
    {
        _context.MailAccesses.Update(mailAccess);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a mail access record
    /// </summary>
    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var mailAccess = await GetByIdAsync(id, cancellationToken);
        if (mailAccess != null)
        {
            _context.MailAccesses.Remove(mailAccess);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
