using NotificationService.Application.Commands;
using NotificationService.Application.DTOs;
using MediatR;

namespace NotificationService.GraphQL.Mutations;

public class Mutation
{
    public async Task<NotificationDto> CreateNotification([Service] IMediator mediator, CreateNotificationDto input)
        => await mediator.Send(new CreateNotificationCommand(input));

    public async Task<NotificationDto> UpdateNotification([Service] IMediator mediator, UpdateNotificationDto input)
        => await mediator.Send(new UpdateNotificationCommand(input));

    public async Task<bool> DeleteNotification([Service] IMediator mediator, int notificationId)
        => await mediator.Send(new DeleteNotificationCommand(notificationId));

    public async Task<NotificationDto> MarkNotificationRead([Service] IMediator mediator, int notificationId, int userId)
        => await mediator.Send(new MarkNotificationReadCommand(notificationId, userId));

    public async Task<NotificationDto> ArchiveNotification([Service] IMediator mediator, int notificationId, int? modifiedBy)
        => await mediator.Send(new ArchiveNotificationCommand(notificationId, modifiedBy));

    public async Task<NotificationCategoryDto> CreateNotificationCategory([Service] IMediator mediator, CreateNotificationCategoryDto input)
        => await mediator.Send(new CreateNotificationCategoryCommand(input));
}
