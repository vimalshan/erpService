namespace UnitService.Domain.Events;

public sealed record EquipmentStatusChangedEvent(
    int EquipmentId,
    string StatusCode,
    string StatusDescription) : DomainEvent;
