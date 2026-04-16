using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace NotificationService.Infrastructure.Repositories;

public class EfNotificationDomainRepository : INotificationDomainRepository
{
    private readonly NotificationDomainDbContext _ctx;
    public EfNotificationDomainRepository(NotificationDomainDbContext ctx) { _ctx = ctx; }

    public async Task<Notification?> GetByIdAsync(int id) =>
        await _ctx.Notifications.Include(n => n.Category).FirstOrDefaultAsync(n => n.NotificationId == id);

    public async Task<IEnumerable<Notification>> GetAllAsync() =>
        await _ctx.Notifications.Include(n => n.Category).OrderByDescending(n => n.CreatedDate).ToListAsync();

    public async Task<IEnumerable<Notification>> GetByCompanyAsync(int companyId) =>
        await _ctx.Notifications.Where(n => n.CompanyId == companyId).OrderByDescending(n => n.CreatedDate).ToListAsync();

    public async Task<Notification> AddAsync(Notification notification)
    {
        _ctx.Notifications.Add(notification); await _ctx.SaveChangesAsync(); return notification;
    }

    public async Task UpdateAsync(Notification notification)
    {
        _ctx.Notifications.Update(notification); await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _ctx.Notifications.FindAsync(id);
        if (entity != null) { _ctx.Notifications.Remove(entity); await _ctx.SaveChangesAsync(); }
    }

    public async Task<IEnumerable<NotificationCategory>> GetCategoriesAsync() =>
        await _ctx.NotificationCategories.Where(c => c.IsActive).OrderBy(c => c.DisplayOrder).ToListAsync();

    public async Task<NotificationCategory> AddCategoryAsync(NotificationCategory category)
    {
        _ctx.NotificationCategories.Add(category); await _ctx.SaveChangesAsync(); return category;
    }
}
