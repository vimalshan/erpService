namespace WebsiteContentService.Domain.Repositories;

using WebsiteContentService.Domain.Entities;

public interface IWebsitePageRepository
{
    Task<WebsitePage?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<WebsitePage?> GetByCodeAsync(string pageCode, CancellationToken ct = default);
    Task<IEnumerable<WebsitePage>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<WebsitePage>> GetPublishedAsync(CancellationToken ct = default);
    Task AddAsync(WebsitePage page, CancellationToken ct = default);
    Task UpdateAsync(WebsitePage page, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}
