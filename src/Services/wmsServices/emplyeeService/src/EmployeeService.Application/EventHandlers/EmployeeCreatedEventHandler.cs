using MediatR;
using Microsoft.Extensions.Logging;
using EmployeeService.Domain.Events;

namespace EmployeeService.Application.EventHandlers;

public sealed class EmployeeCreatedEventHandler : INotificationHandler<EmployeeCreatedEvent>
{
    private readonly ILogger<EmployeeCreatedEventHandler> _logger;

    public EmployeeCreatedEventHandler(ILogger<EmployeeCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(EmployeeCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Employee created - ID: {EmployeeId}, Code: {EmployeeCode}, Name: {FullName}",
            notification.EmployeeId,
            notification.EmployeeCode,
            notification.FullName);

        return Task.CompletedTask;
    }
}
