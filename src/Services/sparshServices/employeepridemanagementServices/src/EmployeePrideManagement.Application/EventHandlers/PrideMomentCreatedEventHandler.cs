using EmployeePrideManagement.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeePrideManagement.Application.EventHandlers;

public class PrideMomentCreatedEventHandler : INotificationHandler<PrideMomentCreatedEvent>
{
    private readonly ILogger<PrideMomentCreatedEventHandler> _logger;

    public PrideMomentCreatedEventHandler(ILogger<PrideMomentCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(PrideMomentCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Pride Moment Created - {Title} for Employee {EmployeeId}",
            notification.PrideMoment.Title,
            notification.PrideMoment.EmployeeSysId);

        return Task.CompletedTask;
    }
}
