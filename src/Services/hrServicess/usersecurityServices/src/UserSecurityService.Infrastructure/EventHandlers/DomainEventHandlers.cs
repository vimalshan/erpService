using MediatR;
using Microsoft.Extensions.Logging;
using UserSecurityService.Domain.Events;
using UserSecurityService.Infrastructure.Services;

namespace UserSecurityService.Infrastructure.EventHandlers;

/// <summary>Handles UserCreatedEvent — publishes to RabbitMQ and logs audit.</summary>
public sealed class UserCreatedEventHandler(
    ILogger<UserCreatedEventHandler> logger)
    : INotificationHandler<DomainEventNotification>
{
    public Task Handle(DomainEventNotification notification, CancellationToken cancellationToken)
    {
        if (notification.DomainEvent is UserCreatedEvent evt)
        {
            logger.LogInformation(
                "[DomainEvent] UserCreated — UserId={UserId}, EmpNum={EmpNum}, Name={Name}",
                evt.UserId, evt.EmpNum, evt.EmpName);
            // Additional: publish to RabbitMQ, send welcome email via SMTP, etc.
        }

        if (notification.DomainEvent is PasswordChangedEvent pwdEvt)
        {
            logger.LogInformation(
                "[DomainEvent] PasswordChanged — UserId={UserId}, EmpSysId={EmpSysId}",
                pwdEvt.UserId, pwdEvt.EmpSysId);
        }

        if (notification.DomainEvent is UserAppMappedEvent mapEvt)
        {
            logger.LogInformation(
                "[DomainEvent] UserAppMapped — EmpSysId={EmpSysId}, App={App}, RoleId={RoleId}",
                mapEvt.EmpSysId, mapEvt.AppCode, mapEvt.HrRoleId);
        }

        if (notification.DomainEvent is UserDeactivatedEvent deactivatedEvt)
        {
            logger.LogInformation(
                "[DomainEvent] UserDeactivated — UserId={UserId}, EmpNum={EmpNum}",
                deactivatedEvt.UserId, deactivatedEvt.EmpNum);
        }

        return Task.CompletedTask;
    }
}
