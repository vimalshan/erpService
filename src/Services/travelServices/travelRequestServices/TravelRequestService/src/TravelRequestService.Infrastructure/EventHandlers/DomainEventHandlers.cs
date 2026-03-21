using MediatR;
using Microsoft.Extensions.Logging;
using TravelRequestService.Domain.Events;
using TravelRequestService.Domain.Interfaces;

namespace TravelRequestService.Infrastructure.EventHandlers;

public class TravelRequestCreatedEventHandler : INotificationHandler<TravelRequestCreatedEvent>
{
    private readonly ILogger<TravelRequestCreatedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public TravelRequestCreatedEventHandler(
        ILogger<TravelRequestCreatedEventHandler> logger,
        IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(TravelRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Travel request {PlanNumber} created for company {CompanyCode}",
            notification.PlanNumber, notification.CompanyCode);

        await _messagePublisher.PublishAsync("travel-request-created", new
        {
            notification.PlanNumber,
            notification.CompanyCode,
            notification.OccurredOn
        }, cancellationToken);
    }
}

public class TravelRequestApprovedEventHandler : INotificationHandler<TravelRequestApprovedEvent>
{
    private readonly ILogger<TravelRequestApprovedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public TravelRequestApprovedEventHandler(
        ILogger<TravelRequestApprovedEventHandler> logger,
        IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(TravelRequestApprovedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Travel request {PlanNumber} approved by {ApprovedBy}",
            notification.PlanNumber, notification.ApprovedBy);

        await _messagePublisher.PublishAsync("travel-request-approved", new
        {
            notification.PlanNumber,
            notification.CompanyCode,
            notification.ApprovedBy,
            notification.OccurredOn
        }, cancellationToken);
    }
}

public class TravelRequestRejectedEventHandler : INotificationHandler<TravelRequestRejectedEvent>
{
    private readonly ILogger<TravelRequestRejectedEventHandler> _logger;

    public TravelRequestRejectedEventHandler(ILogger<TravelRequestRejectedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TravelRequestRejectedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Travel request {PlanNumber} rejected by {RejectedBy}",
            notification.PlanNumber, notification.RejectedBy);

        return Task.CompletedTask;
    }
}

public class TravelRequestCancelledEventHandler : INotificationHandler<TravelRequestCancelledEvent>
{
    private readonly ILogger<TravelRequestCancelledEventHandler> _logger;

    public TravelRequestCancelledEventHandler(ILogger<TravelRequestCancelledEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TravelRequestCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Travel request {PlanNumber} cancelled", notification.PlanNumber);

        return Task.CompletedTask;
    }
}
