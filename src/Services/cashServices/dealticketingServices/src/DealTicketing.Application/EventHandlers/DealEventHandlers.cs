using DealTicketing.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DealTicketing.Application.EventHandlers;

public class DealBatchCreatedEventHandler(ILogger<DealBatchCreatedEventHandler> logger)
    : INotificationHandler<DealBatchCreatedEvent>
{
    public Task Handle(DealBatchCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain event: DealBatch {BatchId} created on {Date} for derivative type {DerType}",
            notification.BatchId, notification.DealDate, notification.DerType);
        return Task.CompletedTask;
    }
}

public class DealCreatedEventHandler(ILogger<DealCreatedEventHandler> logger)
    : INotificationHandler<DealCreatedEvent>
{
    public Task Handle(DealCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain event: Deal {DealId} created in batch {BatchId}. Amount={Amount}, MatDate={MatDate}",
            notification.DealId, notification.BatchId, notification.Amount, notification.MaturityDate);
        return Task.CompletedTask;
    }
}

public class DealApprovedEventHandler(ILogger<DealApprovedEventHandler> logger)
    : INotificationHandler<DealApprovedEvent>
{
    public Task Handle(DealApprovedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain event: Deal {DealId} approved by business unit {Business}",
            notification.DealId, notification.AppBusiness);
        return Task.CompletedTask;
    }
}

public class DealRejectedEventHandler(ILogger<DealRejectedEventHandler> logger)
    : INotificationHandler<DealRejectedEvent>
{
    public Task Handle(DealRejectedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogWarning(
            "Domain event: Deal {DealId} rejected in batch {BatchId}. Reason: {Remarks}",
            notification.DealId, notification.BatchId, notification.Remarks);
        return Task.CompletedTask;
    }
}

public class DealSettledEventHandler(ILogger<DealSettledEventHandler> logger)
    : INotificationHandler<DealSettledEvent>
{
    public Task Handle(DealSettledEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain event: Deal {DealId} settled (settlement {SettlementId}). GainLoss={GainLoss}",
            notification.DealId, notification.SettlementId, notification.GainLossAmt);
        return Task.CompletedTask;
    }
}
