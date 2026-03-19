using EmployeePrideManagement.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeePrideManagement.Application.EventHandlers;

public class PrideMomentUpdatedEventHandler : INotificationHandler<PrideMomentUpdatedEvent>
{
    private readonly ILogger<PrideMomentUpdatedEventHandler> _logger;

    public PrideMomentUpdatedEventHandler(ILogger<PrideMomentUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(PrideMomentUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Pride Moment Updated - {Title} (ID: {Id})",
            notification.PrideMoment.Title,
            notification.PrideMoment.MomentPrideId);

        return Task.CompletedTask;
    }
}
