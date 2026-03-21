namespace UnitService.Domain.Events;

public sealed record EquipmentRegisteredEvent(
    int EquipmentId,
    string EquipmentName,
    string UnitCode) : DomainEvent;
