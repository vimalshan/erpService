using MediatR;

namespace MamAllocationService.Domain.Events;

public record AllocationCreatedEvent(DateTime AllocationDate, int RawMaterialCode) : INotification;

public record AllocationUpdatedEvent(DateTime AllocationDate, int RawMaterialCode, string Field, decimal NewValue) : INotification;

public record AllocationDeletedEvent(DateTime AllocationDate, int RawMaterialCode) : INotification;

public record ArrivalRecordedEvent(long? ArrivalNo, int? ArrivalItem, decimal? ArrivalQty) : INotification;

public record ConsumptionRecordedEvent(long? ConsumptionNo, int? ConsumptionRm, decimal? ConsumptionQty) : INotification;

public record DispatchRecordedEvent(decimal? DispatchNo, int? DispatchFg, decimal? DispatchQty) : INotification;
