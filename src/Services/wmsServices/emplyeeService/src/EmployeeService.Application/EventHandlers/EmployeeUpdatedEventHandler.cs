using MediatR;
using Microsoft.Extensions.Logging;
using EmployeeService.Domain.Events;

namespace EmployeeService.Application.EventHandlers;

public sealed class EmployeeUpdatedEventHandler : INotificationHandler<EmployeeUpdatedEvent>
{
    private readonly ILogger<EmployeeUpdatedEventHandler> _logger;

    public EmployeeUpdatedEventHandler(ILogger<EmployeeUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(EmployeeUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Employee updated - ID: {EmployeeId}, Code: {EmployeeCode}",
            notification.EmployeeId,
            notification.EmployeeCode);

        return Task.CompletedTask;
    }
}
