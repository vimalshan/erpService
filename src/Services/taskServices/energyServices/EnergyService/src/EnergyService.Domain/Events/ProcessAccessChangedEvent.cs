using MediatR;

namespace EnergyService.Domain.Events;

public sealed record ProcessAccessChangedEvent(
    int ProcessId,
    int EmployeeSysId,
    DateTime StartDate,
    DateTime? CloseDate) : INotification;
