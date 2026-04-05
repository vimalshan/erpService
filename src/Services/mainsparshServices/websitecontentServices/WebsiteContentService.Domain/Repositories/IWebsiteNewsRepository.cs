namespace WebsiteContentService.Domain.Repositories;

using WebsiteContentService.Domain.Entities;

public interface IWebsiteNewsRepository
{
    Task<WebsiteNews?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<WebsiteNews>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<WebsiteNews>> GetByCategoryAsync(string category, CancellationToken ct = default);
    Task<IEnumerable<WebsiteNews>> GetPublishedAsync(CancellationToken ct = default);
    Task<IEnumerable<WebsiteNews>> GetFeaturedAsync(CancellationToken ct = default);
    Task AddAsync(WebsiteNews news, CancellationToken ct = default);
    Task UpdateAsync(WebsiteNews news, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}
