using MediatR;
using Microsoft.Extensions.Logging;
using EmployeeService.Domain.Events;

namespace EmployeeService.Application.EventHandlers;

public sealed class EmployeeDeactivatedEventHandler : INotificationHandler<EmployeeDeactivatedEvent>
{
    private readonly ILogger<EmployeeDeactivatedEventHandler> _logger;

    public EmployeeDeactivatedEventHandler(ILogger<EmployeeDeactivatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(EmployeeDeactivatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Employee deactivated - ID: {EmployeeId}, Code: {EmployeeCode}",
            notification.EmployeeId,
            notification.EmployeeCode);

        return Task.CompletedTask;
    }
}
