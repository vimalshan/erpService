using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Interfaces;

public interface IWebsiteContactRepository
{
    Task<WebsiteContactEmail?> GetByIdAsync(long contactId, CancellationToken cancellationToken = default);
    Task<IEnumerable<WebsiteContactEmail>> GetByUserSysIdAsync(long userSysId, CancellationToken cancellationToken = default);
    Task<WebsiteContactEmail?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<WebsiteContactEmail>> GetNewsletterSubscribersAsync(CancellationToken cancellationToken = default);
    Task<WebsiteContactEmail> AddAsync(WebsiteContactEmail contact, CancellationToken cancellationToken = default);
    Task<WebsiteContactEmail> UpdateAsync(WebsiteContactEmail contact, CancellationToken cancellationToken = default);
    Task DeleteAsync(long contactId, CancellationToken cancellationToken = default);
}
