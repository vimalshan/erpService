using MamAllocationService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MamAllocationService.Application.Handlers;

public class AllocationCreatedEventHandler(ILogger<AllocationCreatedEventHandler> logger)
    : INotificationHandler<AllocationCreatedEvent>
{
    public Task Handle(AllocationCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Allocation created for Date={Date}, RM={Rm}",
            notification.AllocationDate, notification.RawMaterialCode);
        return Task.CompletedTask;
    }
}

public class AllocationUpdatedEventHandler(ILogger<AllocationUpdatedEventHandler> logger)
    : INotificationHandler<AllocationUpdatedEvent>
{
    public Task Handle(AllocationUpdatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Allocation updated - Date={Date}, RM={Rm}, Field={Field}, Value={Value}",
            notification.AllocationDate, notification.RawMaterialCode, notification.Field, notification.NewValue);
        return Task.CompletedTask;
    }
}

public class AllocationDeletedEventHandler(ILogger<AllocationDeletedEventHandler> logger)
    : INotificationHandler<AllocationDeletedEvent>
{
    public Task Handle(AllocationDeletedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Allocation deleted for Date={Date}, RM={Rm}",
            notification.AllocationDate, notification.RawMaterialCode);
        return Task.CompletedTask;
    }
}

public class ArrivalRecordedEventHandler(ILogger<ArrivalRecordedEventHandler> logger)
    : INotificationHandler<ArrivalRecordedEvent>
{
    public Task Handle(ArrivalRecordedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Arrival recorded - No={No}, Item={Item}, Qty={Qty}",
            notification.ArrivalNo, notification.ArrivalItem, notification.ArrivalQty);
        return Task.CompletedTask;
    }
}

public class ConsumptionRecordedEventHandler(ILogger<ConsumptionRecordedEventHandler> logger)
    : INotificationHandler<ConsumptionRecordedEvent>
{
    public Task Handle(ConsumptionRecordedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Consumption recorded - No={No}, RM={Rm}, Qty={Qty}",
            notification.ConsumptionNo, notification.ConsumptionRm, notification.ConsumptionQty);
        return Task.CompletedTask;
    }
}

public class DispatchRecordedEventHandler(ILogger<DispatchRecordedEventHandler> logger)
    : INotificationHandler<DispatchRecordedEvent>
{
    public Task Handle(DispatchRecordedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Dispatch recorded - No={No}, FG={Fg}, Qty={Qty}",
            notification.DispatchNo, notification.DispatchFg, notification.DispatchQty);
        return Task.CompletedTask;
    }
}
