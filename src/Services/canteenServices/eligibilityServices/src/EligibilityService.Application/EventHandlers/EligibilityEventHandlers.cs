using MediatR;
using Microsoft.Extensions.Logging;
using EligibilityService.Domain.Events;

namespace EligibilityService.Application.EventHandlers;

public class EligibilityCreatedEventHandler : INotificationHandler<EligibilityCreatedEvent>
{
    private readonly ILogger<EligibilityCreatedEventHandler> _logger;

    public EligibilityCreatedEventHandler(ILogger<EligibilityCreatedEventHandler> logger)
        => _logger = logger;

    public Task Handle(EligibilityCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: EligibilityMaster created — CanteenUnit={CanteenUnit}, ShiftCode={ShiftCode}, ItemCode={ItemCode}",
            notification.EligibilityMaster.CanteenUnit,
            notification.EligibilityMaster.ShiftCode,
            notification.EligibilityMaster.ItemCode);
        return Task.CompletedTask;
    }
}

public class EligibilityUpdatedEventHandler : INotificationHandler<EligibilityUpdatedEvent>
{
    private readonly ILogger<EligibilityUpdatedEventHandler> _logger;

    public EligibilityUpdatedEventHandler(ILogger<EligibilityUpdatedEventHandler> logger)
        => _logger = logger;

    public Task Handle(EligibilityUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: EligibilityMaster updated — CanteenUnit={CanteenUnit}, ShiftCode={ShiftCode}, ModifiedUser={ModifiedUser}",
            notification.EligibilityMaster.CanteenUnit,
            notification.EligibilityMaster.ShiftCode,
            notification.ModifiedUser);
        return Task.CompletedTask;
    }
}

public class EligibilityDeletedEventHandler : INotificationHandler<EligibilityDeletedEvent>
{
    private readonly ILogger<EligibilityDeletedEventHandler> _logger;

    public EligibilityDeletedEventHandler(ILogger<EligibilityDeletedEventHandler> logger)
        => _logger = logger;

    public Task Handle(EligibilityDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: EligibilityMaster deleted — CanteenUnit={CanteenUnit}, ShiftCode={ShiftCode}, ItemCode={ItemCode}",
            notification.CanteenUnit, notification.ShiftCode, notification.ItemCode);
        return Task.CompletedTask;
    }
}
