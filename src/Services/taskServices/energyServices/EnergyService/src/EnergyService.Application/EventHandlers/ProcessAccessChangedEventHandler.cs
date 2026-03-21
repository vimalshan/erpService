using EnergyService.Application.Common.Interfaces;
using EnergyService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnergyService.Application.EventHandlers;

public class ProcessAccessChangedEventHandler : INotificationHandler<ProcessAccessChangedEvent>
{
    private readonly IRabbitMqPublisher _publisher;
    private readonly ILogger<ProcessAccessChangedEventHandler> _logger;

    public ProcessAccessChangedEventHandler(IRabbitMqPublisher publisher, ILogger<ProcessAccessChangedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(ProcessAccessChangedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: Process access changed for Process {ProcessId}, Employee {EmployeeId}",
            notification.ProcessId, notification.EmployeeSysId);

        await _publisher.PublishAsync("energy-exchange", "access.changed", notification, ct);
    }
}
