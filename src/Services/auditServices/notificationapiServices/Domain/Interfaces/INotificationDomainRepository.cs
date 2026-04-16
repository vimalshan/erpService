using NotificationService.Domain.Entities;

namespace NotificationService.Domain.Interfaces;

public interface INotificationDomainRepository
{
    Task<Notification?> GetByIdAsync(int id);
    Task<IEnumerable<Notification>> GetAllAsync();
    Task<IEnumerable<Notification>> GetByCompanyAsync(int companyId);
    Task<Notification> AddAsync(Notification notification);
    Task UpdateAsync(Notification notification);
    Task DeleteAsync(int id);
    Task<IEnumerable<NotificationCategory>> GetCategoriesAsync();
    Task<NotificationCategory> AddCategoryAsync(NotificationCategory category);
}
