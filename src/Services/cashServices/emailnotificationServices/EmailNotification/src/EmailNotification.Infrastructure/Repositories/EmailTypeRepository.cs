using Microsoft.EntityFrameworkCore;
using EmailNotification.Domain.Aggregates;
using EmailNotification.Domain.Repositories;
using EmailNotification.Application.Services;

namespace EmailNotification.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for EmailType aggregate
/// </summary>
public class EmailTypeRepository : IEmailTypeRepository
{
    private readonly Data.EmailNotificationDbContext _context;
    private readonly IDomainEventDispatcher _eventDispatcher;

    /// <summary>
    /// Initializes a new instance of the EmailTypeRepository class
    /// </summary>
    /// <param name="context">The DbContext</param>
    /// <param name="eventDispatcher">The domain event dispatcher</param>
    public EmailTypeRepository(Data.EmailNotificationDbContext context, IDomainEventDispatcher eventDispatcher)
    {
        _context = context;
        _eventDispatcher = eventDispatcher;
    }

    /// <summary>
    /// Gets an email type by its ID
    /// </summary>
    public async Task<EmailTypeAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.EmailTypes
            .Include(x => x.MailAccessList)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <summary>
    /// Gets all email types
    /// </summary>
    public async Task<IEnumerable<EmailTypeAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.EmailTypes
            .Include(x => x.MailAccessList)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets email types by type (Daily or Event)
    /// </summary>
    public async Task<IEnumerable<EmailTypeAggregate>> GetByTypeAsync(
        Domain.ValueObjects.EmailTypeEnum emailType,
        CancellationToken cancellationToken = default)
    {
        return await _context.EmailTypes
            .Include(x => x.MailAccessList)
            .Where(x => x.EmailType == emailType)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Adds a new email type
    /// </summary>
    public async Task AddAsync(EmailTypeAggregate emailType, CancellationToken cancellationToken = default)
    {
        _context.EmailTypes.Add(emailType);
        await _context.SaveChangesAsync(cancellationToken);
        
        // Dispatch domain events after saving
        await _eventDispatcher.DispatchEventsAsync(emailType, cancellationToken);
    }

    /// <summary>
    /// Updates an existing email type
    /// </summary>
    public async Task UpdateAsync(EmailTypeAggregate emailType, CancellationToken cancellationToken = default)
    {
        _context.EmailTypes.Update(emailType);
        await _context.SaveChangesAsync(cancellationToken);
        
        // Dispatch domain events after saving
        await _eventDispatcher.DispatchEventsAsync(emailType, cancellationToken);
    }

    /// <summary>
    /// Deletes an email type
    /// </summary>
    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var emailType = await GetByIdAsync(id, cancellationToken);
        if (emailType != null)
        {
            _context.EmailTypes.Remove(emailType);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
