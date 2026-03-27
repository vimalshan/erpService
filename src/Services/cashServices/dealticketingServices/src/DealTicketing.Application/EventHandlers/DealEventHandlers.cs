using DealTicketing.Application.Contracts;
using DealTicketing.Domain.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DealTicketing.Application.EventHandlers;

public class DealBatchCreatedEventHandler(ILogger<DealBatchCreatedEventHandler> logger, IPublishEndpoint publishEndpoint)
    : INotificationHandler<DealBatchCreatedEvent>
{
    public async Task Handle(DealBatchCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain event: DealBatch {BatchId} created on {Date} for derivative type {DerType}",
            notification.BatchId, notification.DealDate, notification.DerType);

        await publishEndpoint.Publish(new DealBatchCreatedMessage(
            notification.BatchId, notification.DealDate, notification.DerType, DateTime.UtcNow), cancellationToken);
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

public class DealApprovedEventHandler(ILogger<DealApprovedEventHandler> logger, IPublishEndpoint publishEndpoint)
    : INotificationHandler<DealApprovedEvent>
{
    public async Task Handle(DealApprovedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain event: Deal {DealId} approved by business unit {Business}",
            notification.DealId, notification.AppBusiness);

        await publishEndpoint.Publish(new DealApprovedMessage(
            notification.DealId, notification.BatchId, notification.AppBusiness, DateTime.UtcNow), cancellationToken);
    }
}

public class DealRejectedEventHandler(ILogger<DealRejectedEventHandler> logger, IPublishEndpoint publishEndpoint)
    : INotificationHandler<DealRejectedEvent>
{
    public async Task Handle(DealRejectedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogWarning(
            "Domain event: Deal {DealId} rejected in batch {BatchId}. Reason: {Remarks}",
            notification.DealId, notification.BatchId, notification.Remarks);

        await publishEndpoint.Publish(new DealRejectedMessage(
            notification.DealId, notification.BatchId, notification.Remarks, DateTime.UtcNow), cancellationToken);
    }
}

public class DealSettledEventHandler(ILogger<DealSettledEventHandler> logger, IPublishEndpoint publishEndpoint)
    : INotificationHandler<DealSettledEvent>
{
    public async Task Handle(DealSettledEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain event: Deal {DealId} settled (settlement {SettlementId}). GainLoss={GainLoss}",
            notification.DealId, notification.SettlementId, notification.GainLossAmt);

        await publishEndpoint.Publish(new DealSettledMessage(
            notification.DealId, notification.SettlementId, notification.GainLossAmt, DateTime.UtcNow), cancellationToken);
    }
}
