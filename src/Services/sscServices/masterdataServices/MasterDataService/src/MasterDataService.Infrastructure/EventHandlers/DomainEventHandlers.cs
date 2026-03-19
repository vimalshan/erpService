using MasterDataService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MasterDataService.Infrastructure.EventHandlers;

public class LovMasterCreatedEventHandler(ILogger<LovMasterCreatedEventHandler> logger)
    : INotificationHandler<LovMasterCreatedEvent>
{
    public Task Handle(LovMasterCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: LOV Master created - Id: {LovId}, Type: {LovType}, Name: {LovName}",
            notification.LovId, notification.LovType, notification.LovName);
        return Task.CompletedTask;
    }
}

public class LovMasterUpdatedEventHandler(ILogger<LovMasterUpdatedEventHandler> logger)
    : INotificationHandler<LovMasterUpdatedEvent>
{
    public Task Handle(LovMasterUpdatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: LOV Master updated - Id: {LovId}, Type: {LovType}, Name: {LovName}",
            notification.LovId, notification.LovType, notification.LovName);
        return Task.CompletedTask;
    }
}

public class HoldTypeMasterCreatedEventHandler(ILogger<HoldTypeMasterCreatedEventHandler> logger)
    : INotificationHandler<HoldTypeMasterCreatedEvent>
{
    public Task Handle(HoldTypeMasterCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Hold Type Master created - Id: {HoldId}, Name: {HoldName}",
            notification.HoldId, notification.HoldName);
        return Task.CompletedTask;
    }
}

public class ScannerMasterCreatedEventHandler(ILogger<ScannerMasterCreatedEventHandler> logger)
    : INotificationHandler<ScannerMasterCreatedEvent>
{
    public Task Handle(ScannerMasterCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Scanner Master created - Id: {DeviceId}, Name: {DeviceName}",
            notification.DeviceId, notification.DeviceName);
        return Task.CompletedTask;
    }
}
