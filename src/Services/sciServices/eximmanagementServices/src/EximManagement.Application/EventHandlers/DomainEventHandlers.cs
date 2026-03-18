using EximManagement.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EximManagement.Application.EventHandlers;

public class EximDataFileUploadedEventHandler(ILogger<EximDataFileUploadedEventHandler> logger)
    : INotificationHandler<EximDataFileUploadedEvent>
{
    public Task Handle(EximDataFileUploadedEvent notification, CancellationToken ct)
    {
        logger.LogInformation(
            "Domain Event: EximDataFile uploaded. FileId={FileId}, FileType={FileType}, At={OccurredOn}",
            notification.FileId, notification.FileType, notification.OccurredOn);
        return Task.CompletedTask;
    }
}

public class EximDataFileDeletedEventHandler(ILogger<EximDataFileDeletedEventHandler> logger)
    : INotificationHandler<EximDataFileDeletedEvent>
{
    public Task Handle(EximDataFileDeletedEvent notification, CancellationToken ct)
    {
        logger.LogInformation(
            "Domain Event: EximDataFile deleted. FileId={FileId}, At={OccurredOn}",
            notification.FileId, notification.OccurredOn);
        return Task.CompletedTask;
    }
}

public class EximProductCreatedEventHandler(ILogger<EximProductCreatedEventHandler> logger)
    : INotificationHandler<EximProductCreatedEvent>
{
    public Task Handle(EximProductCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation(
            "Domain Event: EximProduct created. ProductId={ProductId}, ProductName={ProductName}, At={OccurredOn}",
            notification.ProductId, notification.ProductName, notification.OccurredOn);
        return Task.CompletedTask;
    }
}

public class EximProductGroupCreatedEventHandler(ILogger<EximProductGroupCreatedEventHandler> logger)
    : INotificationHandler<EximProductGroupCreatedEvent>
{
    public Task Handle(EximProductGroupCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation(
            "Domain Event: ProductGroup created. GroupId={GroupId}, GroupName={GroupName}",
            notification.GroupId, notification.GroupName);
        return Task.CompletedTask;
    }
}

public class EximDataProcessedEventHandler(ILogger<EximDataProcessedEventHandler> logger)
    : INotificationHandler<EximDataProcessedEvent>
{
    public Task Handle(EximDataProcessedEvent notification, CancellationToken ct)
    {
        logger.LogInformation(
            "Domain Event: EximData processed. FileId={FileId}, Type={FileType}, Count={RecordCount}",
            notification.FileId, notification.FileType, notification.RecordCount);
        return Task.CompletedTask;
    }
}
