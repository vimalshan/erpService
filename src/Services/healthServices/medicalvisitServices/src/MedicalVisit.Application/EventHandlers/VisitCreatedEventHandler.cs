using MediatR;
using Microsoft.Extensions.Logging;
using MedicalVisit.Domain.Events;
using MedicalVisit.Application.Common.Interfaces;

namespace MedicalVisit.Application.EventHandlers;

public class VisitCreatedEventHandler : INotificationHandler<VisitCreatedEvent>
{
    private readonly ILogger<VisitCreatedEventHandler> _logger;
    private readonly IEventPublisher _publisher;

    public VisitCreatedEventHandler(ILogger<VisitCreatedEventHandler> logger, IEventPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public Task Handle(VisitCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Visit created: {CompanyCode}/{VisitNumber} on {VisitDate}",
            notification.Visit.CompanyCode,
            notification.Visit.VisitNumber,
            notification.Visit.VisitDate);

        _publisher.Publish(
            "medical.visit.events",
            "visit.created",
            new
            {
                CompanyCode = notification.Visit.CompanyCode,
                notification.Visit.VisitNumber,
                notification.Visit.VisitDate,
                notification.Visit.DoctorCode,
                OccurredOn = notification.OccurredOn
            });

        return Task.CompletedTask;
    }
}
