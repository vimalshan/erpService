using NotificationService.Application.DTOs;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;
using MediatR;

namespace NotificationService.Application.Commands;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, NotificationDto>
{
    private readonly INotificationDomainRepository _repo;
    private readonly IMediator _mediator;
    public CreateNotificationCommandHandler(INotificationDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<NotificationDto> Handle(CreateNotificationCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var entity = Notification.Create(d.Title, d.Message, d.CategoryId, d.Priority,
            d.CompanyId, d.SiteId, d.ServiceId, d.TargetAudience, d.CreatedBy);
        entity.ExpiryDate = d.ExpiryDate; entity.ActionRequired = d.ActionRequired;
        entity.ActionUrl = d.ActionUrl; entity.AttachmentPath = d.AttachmentPath;
        entity.RelatedEntityType = d.RelatedEntityType; entity.RelatedEntityId = d.RelatedEntityId;

        var created = await _repo.AddAsync(entity);
        foreach (var evt in created.DomainEvents) await _mediator.Publish(evt, ct);
        created.ClearDomainEvents();
        return MapToDto(created);
    }

    private static NotificationDto MapToDto(Notification n) => new(
        n.NotificationId, n.Title, n.Message, n.CategoryId, n.CompanyId, n.SiteId, n.ServiceId,
        n.Priority, n.Status, n.CreatedDate, n.ModifiedDate, n.CreatedBy, n.ModifiedBy,
        n.ExpiryDate, n.IsActive, n.TargetAudience, n.ActionRequired, n.ActionUrl,
        n.AttachmentPath, n.RelatedEntityType, n.RelatedEntityId);
}

public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, NotificationDto>
{
    private readonly INotificationDomainRepository _repo;
    public UpdateNotificationCommandHandler(INotificationDomainRepository repo) { _repo = repo; }

    public async Task<NotificationDto> Handle(UpdateNotificationCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var e = await _repo.GetByIdAsync(d.NotificationId) ?? throw new System.Collections.Generic.KeyNotFoundException($"Notification {d.NotificationId} not found");
        e.Title = d.Title; e.Message = d.Message; e.CategoryId = d.CategoryId;
        e.CompanyId = d.CompanyId; e.SiteId = d.SiteId; e.ServiceId = d.ServiceId;
        e.Priority = d.Priority; e.Status = d.Status; e.ExpiryDate = d.ExpiryDate;
        e.IsActive = d.IsActive; e.TargetAudience = d.TargetAudience; e.ActionRequired = d.ActionRequired;
        e.ActionUrl = d.ActionUrl; e.AttachmentPath = d.AttachmentPath;
        e.RelatedEntityType = d.RelatedEntityType; e.RelatedEntityId = d.RelatedEntityId;
        e.ModifiedDate = DateTime.UtcNow; e.ModifiedBy = d.ModifiedBy;
        await _repo.UpdateAsync(e);
        return new NotificationDto(e.NotificationId, e.Title, e.Message, e.CategoryId, e.CompanyId,
            e.SiteId, e.ServiceId, e.Priority, e.Status, e.CreatedDate, e.ModifiedDate,
            e.CreatedBy, e.ModifiedBy, e.ExpiryDate, e.IsActive, e.TargetAudience,
            e.ActionRequired, e.ActionUrl, e.AttachmentPath, e.RelatedEntityType, e.RelatedEntityId);
    }
}

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, bool>
{
    private readonly INotificationDomainRepository _repo;
    public DeleteNotificationCommandHandler(INotificationDomainRepository repo) { _repo = repo; }
    public async Task<bool> Handle(DeleteNotificationCommand request, CancellationToken ct)
    {
        await _repo.DeleteAsync(request.NotificationId); return true;
    }
}

public class MarkNotificationReadCommandHandler : IRequestHandler<MarkNotificationReadCommand, NotificationDto>
{
    private readonly INotificationDomainRepository _repo;
    private readonly IMediator _mediator;
    public MarkNotificationReadCommandHandler(INotificationDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<NotificationDto> Handle(MarkNotificationReadCommand request, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(request.NotificationId) ?? throw new System.Collections.Generic.KeyNotFoundException($"Notification {request.NotificationId} not found");
        e.MarkRead(request.UserId);
        await _repo.UpdateAsync(e);
        foreach (var evt in e.DomainEvents) await _mediator.Publish(evt, ct);
        e.ClearDomainEvents();
        return new NotificationDto(e.NotificationId, e.Title, e.Message, e.CategoryId, e.CompanyId,
            e.SiteId, e.ServiceId, e.Priority, e.Status, e.CreatedDate, e.ModifiedDate,
            e.CreatedBy, e.ModifiedBy, e.ExpiryDate, e.IsActive, e.TargetAudience,
            e.ActionRequired, e.ActionUrl, e.AttachmentPath, e.RelatedEntityType, e.RelatedEntityId);
    }
}

public class ArchiveNotificationCommandHandler : IRequestHandler<ArchiveNotificationCommand, NotificationDto>
{
    private readonly INotificationDomainRepository _repo;
    private readonly IMediator _mediator;
    public ArchiveNotificationCommandHandler(INotificationDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<NotificationDto> Handle(ArchiveNotificationCommand request, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(request.NotificationId) ?? throw new System.Collections.Generic.KeyNotFoundException($"Notification {request.NotificationId} not found");
        e.Archive(request.ModifiedBy);
        await _repo.UpdateAsync(e);
        foreach (var evt in e.DomainEvents) await _mediator.Publish(evt, ct);
        e.ClearDomainEvents();
        return new NotificationDto(e.NotificationId, e.Title, e.Message, e.CategoryId, e.CompanyId,
            e.SiteId, e.ServiceId, e.Priority, e.Status, e.CreatedDate, e.ModifiedDate,
            e.CreatedBy, e.ModifiedBy, e.ExpiryDate, e.IsActive, e.TargetAudience,
            e.ActionRequired, e.ActionUrl, e.AttachmentPath, e.RelatedEntityType, e.RelatedEntityId);
    }
}

public class CreateNotificationCategoryCommandHandler : IRequestHandler<CreateNotificationCategoryCommand, NotificationCategoryDto>
{
    private readonly INotificationDomainRepository _repo;
    public CreateNotificationCategoryCommandHandler(INotificationDomainRepository repo) { _repo = repo; }

    public async Task<NotificationCategoryDto> Handle(CreateNotificationCategoryCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var entity = new NotificationCategory
        {
            CategoryName = d.CategoryName, CategoryCode = d.CategoryCode, Description = d.Description,
            Color = d.Color, Icon = d.Icon, Priority = d.Priority, DisplayOrder = d.DisplayOrder,
            IsActive = true, CreatedBy = d.CreatedBy, ModifiedBy = d.CreatedBy,
            CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow
        };
        var created = await _repo.AddCategoryAsync(entity);
        return new NotificationCategoryDto(created.CategoryId, created.CategoryName, created.CategoryCode,
            created.Description, created.IsActive, created.Color, created.Icon, created.Priority, created.DisplayOrder);
    }
}
