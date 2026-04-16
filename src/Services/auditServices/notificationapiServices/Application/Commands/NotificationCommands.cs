using NotificationService.Application.DTOs;
using MediatR;

namespace NotificationService.Application.Commands;

public record CreateNotificationCommand(CreateNotificationDto Dto) : IRequest<NotificationDto>;
public record UpdateNotificationCommand(UpdateNotificationDto Dto) : IRequest<NotificationDto>;
public record DeleteNotificationCommand(int NotificationId) : IRequest<bool>;
public record MarkNotificationReadCommand(int NotificationId, int UserId) : IRequest<NotificationDto>;
public record ArchiveNotificationCommand(int NotificationId, int? ModifiedBy) : IRequest<NotificationDto>;
public record CreateNotificationCategoryCommand(CreateNotificationCategoryDto Dto) : IRequest<NotificationCategoryDto>;
