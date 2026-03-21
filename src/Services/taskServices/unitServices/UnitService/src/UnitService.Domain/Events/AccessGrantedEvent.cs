namespace UnitService.Domain.Events;

public sealed record AccessGrantedEvent(
    int EmployeeSysId,
    string UnitCode,
    string AccessType) : DomainEvent;
