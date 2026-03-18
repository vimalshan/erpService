using MediatR;
using Microsoft.Extensions.Logging;
using MedicalVisit.Domain.Events;
using MedicalVisit.Application.Common.Interfaces;

namespace MedicalVisit.Application.EventHandlers;

public class VisitCancelledEventHandler : INotificationHandler<VisitCancelledEvent>
{
    private readonly ILogger<VisitCancelledEventHandler> _logger;
    private readonly IEventPublisher _publisher;

    public VisitCancelledEventHandler(ILogger<VisitCancelledEventHandler> logger, IEventPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public Task Handle(VisitCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Visit cancelled: {CompanyCode}/{VisitNumber}",
            notification.Visit.CompanyCode,
            notification.Visit.VisitNumber);

        _publisher.Publish(
            "medical.visit.events",
            "visit.cancelled",
            new
            {
                CompanyCode = notification.Visit.CompanyCode,
                notification.Visit.VisitNumber,
                notification.Visit.VisitDate,
                OccurredOn = notification.OccurredOn
            });

        return Task.CompletedTask;
    }
}
