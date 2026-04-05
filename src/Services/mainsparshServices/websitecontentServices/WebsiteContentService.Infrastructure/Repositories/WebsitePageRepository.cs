namespace WebsiteContentService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using WebsiteContentService.Domain.Entities;
using WebsiteContentService.Domain.Repositories;
using WebsiteContentService.Infrastructure.Persistence;

public class WebsitePageRepository(WebsiteContentDbContext context) : IWebsitePageRepository
{
    public async Task<WebsitePage?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.WebsitePages.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<WebsitePage?> GetByCodeAsync(string pageCode, CancellationToken ct = default)
        => await context.WebsitePages.AsNoTracking()
            .FirstOrDefaultAsync(p => p.PageCode.Value == pageCode, ct);

    public async Task<IEnumerable<WebsitePage>> GetAllAsync(CancellationToken ct = default)
        => await context.WebsitePages.AsNoTracking()
            .OrderBy(p => p.PageOrder).ThenBy(p => p.Id)
            .ToListAsync(ct);

    public async Task<IEnumerable<WebsitePage>> GetPublishedAsync(CancellationToken ct = default)
        => await context.WebsitePages.AsNoTracking()
            .Where(p => p.IsPublished.Value == 'Y')
            .OrderBy(p => p.PageOrder).ThenBy(p => p.Id)
            .ToListAsync(ct);

    public async Task AddAsync(WebsitePage page, CancellationToken ct = default)
        => await context.WebsitePages.AddAsync(page, ct);

    public Task UpdateAsync(WebsitePage page, CancellationToken ct = default)
    {
        context.WebsitePages.Update(page);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var page = await GetByIdAsync(id, ct);
        if (page is not null) context.WebsitePages.Remove(page);
    }
}
