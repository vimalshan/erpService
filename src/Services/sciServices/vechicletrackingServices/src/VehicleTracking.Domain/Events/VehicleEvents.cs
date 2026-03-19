using VehicleTracking.Domain.Common;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Domain.Events;

public class VehicleRegisteredEvent(VehicleMaster vehicle) : DomainEvent
{
    public VehicleMaster Vehicle { get; } = vehicle;
}

public class VehicleStageUpdatedEvent(long trackingNumber, long stageCode) : DomainEvent
{
    public long TrackingNumber { get; } = trackingNumber;
    public long StageCode { get; } = stageCode;
}

public class VehicleTransactionCreatedEvent(long trackingNumber) : DomainEvent
{
    public long TrackingNumber { get; } = trackingNumber;
}

public class DecisionMadeEvent(long trackingNumber, long purposeCode, long stageCode, char decision) : DomainEvent
{
    public long TrackingNumber { get; } = trackingNumber;
    public long PurposeCode { get; } = purposeCode;
    public long StageCode { get; } = stageCode;
    public char Decision { get; } = decision;
}
