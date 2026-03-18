using ContributionService.Application.Interfaces;
using ContributionService.Domain.Events;
using MediatR;

namespace ContributionService.API.Extensions;

public class ContributionBatchCreatedEventHandler(
    ILogger<ContributionBatchCreatedEventHandler> logger,
    IServiceScopeFactory scopeFactory)
    : INotificationHandler<ContributionBatchCreatedEvent>
{
    public async Task Handle(ContributionBatchCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Contribution batch {BatchNo} created for trust {TrustCode}",
            notification.BatchNo, notification.TrustCode);

        try
        {
            using var scope = scopeFactory.CreateScope();
            var publisher = scope.ServiceProvider.GetService<IMessagePublisher>();
            if (publisher != null)
            {
                await publisher.PublishAsync("contribution.exchange", "contribution.created",
                    new { notification.BatchNo, notification.TrustCode, notification.PayunitCode, Timestamp = DateTime.UtcNow }, ct);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to publish ContributionBatchCreated event to message bus");
        }
    }
}

public class ContributionBatchPostedEventHandler(ILogger<ContributionBatchPostedEventHandler> logger)
    : INotificationHandler<ContributionBatchPostedEvent>
{
    public Task Handle(ContributionBatchPostedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Contribution batch {BatchNo} posted by user {UserId}",
            notification.BatchNo, notification.PostedByUserId);
        return Task.CompletedTask;
    }
}

public class ContributionStatusChangedEventHandler(ILogger<ContributionStatusChangedEventHandler> logger)
    : INotificationHandler<ContributionStatusChangedEvent>
{
    public Task Handle(ContributionStatusChangedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Contribution batch {BatchNo} status changed to {Status}",
            notification.BatchNo, notification.NewStatus);
        return Task.CompletedTask;
    }
}

public class MonthlyContributionProcessedEventHandler(ILogger<MonthlyContributionProcessedEventHandler> logger)
    : INotificationHandler<MonthlyContributionProcessedEvent>
{
    public Task Handle(MonthlyContributionProcessedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Monthly contribution processed for {MonthYear}, rows: {Rows}",
            notification.MonthYear, notification.RowsProcessed);
        return Task.CompletedTask;
    }
}

public class SuperannuationBatchCreatedEventHandler(ILogger<SuperannuationBatchCreatedEventHandler> logger)
    : INotificationHandler<SuperannuationBatchCreatedEvent>
{
    public Task Handle(SuperannuationBatchCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Superannuation batch {BatchNo} created", notification.BatchNo);
        return Task.CompletedTask;
    }
}
