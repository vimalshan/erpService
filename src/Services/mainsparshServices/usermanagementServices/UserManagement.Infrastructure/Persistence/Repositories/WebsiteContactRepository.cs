using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Infrastructure.Persistence.Repositories;

public class WebsiteContactRepository(ApplicationDbContext context) : IWebsiteContactRepository
{
    public async Task<WebsiteContactEmail?> GetByIdAsync(long contactId, CancellationToken cancellationToken = default)
        => await context.WebsiteContactEmails.FindAsync([contactId], cancellationToken);

    public async Task<IEnumerable<WebsiteContactEmail>> GetByUserSysIdAsync(long userSysId, CancellationToken cancellationToken = default)
        => await context.WebsiteContactEmails
            .AsNoTracking()
            .Where(c => c.UserSysId == userSysId)
            .ToListAsync(cancellationToken);

    public async Task<WebsiteContactEmail?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await context.WebsiteContactEmails
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.PrimaryEmail == email.ToLowerInvariant(), cancellationToken);

    public async Task<IEnumerable<WebsiteContactEmail>> GetNewsletterSubscribersAsync(CancellationToken cancellationToken = default)
        => await context.WebsiteContactEmails
            .AsNoTracking()
            .Where(c => c.NewsletterOptIn == 'Y' && c.ContactStatus == 'A')
            .ToListAsync(cancellationToken);

    public async Task<WebsiteContactEmail> AddAsync(WebsiteContactEmail contact, CancellationToken cancellationToken = default)
    {
        await context.WebsiteContactEmails.AddAsync(contact, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return contact;
    }

    public async Task<WebsiteContactEmail> UpdateAsync(WebsiteContactEmail contact, CancellationToken cancellationToken = default)
    {
        context.WebsiteContactEmails.Update(contact);
        await context.SaveChangesAsync(cancellationToken);
        return contact;
    }

    public async Task DeleteAsync(long contactId, CancellationToken cancellationToken = default)
    {
        var contact = await context.WebsiteContactEmails.FindAsync([contactId]);
        if (contact is not null)
        {
            context.WebsiteContactEmails.Remove(contact);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
