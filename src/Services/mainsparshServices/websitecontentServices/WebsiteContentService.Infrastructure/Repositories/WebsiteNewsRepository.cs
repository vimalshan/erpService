namespace WebsiteContentService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using WebsiteContentService.Domain.Entities;
using WebsiteContentService.Domain.Repositories;
using WebsiteContentService.Infrastructure.Persistence;

public class WebsiteNewsRepository(WebsiteContentDbContext context) : IWebsiteNewsRepository
{
    public async Task<WebsiteNews?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.WebsiteNews.FirstOrDefaultAsync(n => n.Id == id, ct);

    public async Task<IEnumerable<WebsiteNews>> GetAllAsync(CancellationToken ct = default)
        => await context.WebsiteNews.AsNoTracking()
            .OrderByDescending(n => n.CreatedOn)
            .ToListAsync(ct);

    public async Task<IEnumerable<WebsiteNews>> GetByCategoryAsync(string category, CancellationToken ct = default)
        => await context.WebsiteNews.AsNoTracking()
            .Where(n => n.NewsCategory == category)
            .OrderByDescending(n => n.CreatedOn)
            .ToListAsync(ct);

    public async Task<IEnumerable<WebsiteNews>> GetPublishedAsync(CancellationToken ct = default)
        => await context.WebsiteNews.AsNoTracking()
            .Where(n => n.IsPublished.Value == 'Y')
            .OrderByDescending(n => n.PublishedDate)
            .ToListAsync(ct);

    public async Task<IEnumerable<WebsiteNews>> GetFeaturedAsync(CancellationToken ct = default)
        => await context.WebsiteNews.AsNoTracking()
            .Where(n => n.IsFeatured.Value == 'Y' && n.IsPublished.Value == 'Y')
            .OrderByDescending(n => n.PublishedDate)
            .ToListAsync(ct);

    public async Task AddAsync(WebsiteNews news, CancellationToken ct = default)
        => await context.WebsiteNews.AddAsync(news, ct);

    public Task UpdateAsync(WebsiteNews news, CancellationToken ct = default)
    {
        context.WebsiteNews.Update(news);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var news = await GetByIdAsync(id, ct);
        if (news is not null) context.WebsiteNews.Remove(news);
    }
}
