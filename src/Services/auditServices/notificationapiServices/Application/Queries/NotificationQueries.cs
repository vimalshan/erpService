using NotificationService.Application.DTOs;
using NotificationService.Domain.Interfaces;
using MediatR;

namespace NotificationService.Application.Queries;

public record GetNotificationByIdQuery(int NotificationId) : IRequest<NotificationDto?>;
public record GetAllNotificationsQuery() : IRequest<IEnumerable<NotificationDto>>;
public record GetNotificationCategoriesQuery() : IRequest<IEnumerable<NotificationCategoryDto>>;

public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, NotificationDto?>
{
    private readonly INotificationDomainRepository _repo;
    public GetNotificationByIdQueryHandler(INotificationDomainRepository repo) { _repo = repo; }

    public async Task<NotificationDto?> Handle(GetNotificationByIdQuery request, CancellationToken ct)
    {
        var n = await _repo.GetByIdAsync(request.NotificationId);
        if (n == null) return null;
        return new NotificationDto(n.NotificationId, n.Title, n.Message, n.CategoryId, n.CompanyId,
            n.SiteId, n.ServiceId, n.Priority, n.Status, n.CreatedDate, n.ModifiedDate,
            n.CreatedBy, n.ModifiedBy, n.ExpiryDate, n.IsActive, n.TargetAudience,
            n.ActionRequired, n.ActionUrl, n.AttachmentPath, n.RelatedEntityType, n.RelatedEntityId);
    }
}

public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, IEnumerable<NotificationDto>>
{
    private readonly INotificationDomainRepository _repo;
    public GetAllNotificationsQueryHandler(INotificationDomainRepository repo) { _repo = repo; }

    public async Task<IEnumerable<NotificationDto>> Handle(GetAllNotificationsQuery request, CancellationToken ct)
    {
        var list = await _repo.GetAllAsync();
        return list.Select(n => new NotificationDto(n.NotificationId, n.Title, n.Message, n.CategoryId,
            n.CompanyId, n.SiteId, n.ServiceId, n.Priority, n.Status, n.CreatedDate, n.ModifiedDate,
            n.CreatedBy, n.ModifiedBy, n.ExpiryDate, n.IsActive, n.TargetAudience,
            n.ActionRequired, n.ActionUrl, n.AttachmentPath, n.RelatedEntityType, n.RelatedEntityId));
    }
}

public class GetNotificationCategoriesQueryHandler : IRequestHandler<GetNotificationCategoriesQuery, IEnumerable<NotificationCategoryDto>>
{
    private readonly INotificationDomainRepository _repo;
    public GetNotificationCategoriesQueryHandler(INotificationDomainRepository repo) { _repo = repo; }

    public async Task<IEnumerable<NotificationCategoryDto>> Handle(GetNotificationCategoriesQuery request, CancellationToken ct)
    {
        var list = await _repo.GetCategoriesAsync();
        return list.Select(c => new NotificationCategoryDto(c.CategoryId, c.CategoryName, c.CategoryCode,
            c.Description, c.IsActive, c.Color, c.Icon, c.Priority, c.DisplayOrder));
    }
}
