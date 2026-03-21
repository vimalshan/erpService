using MediatR;

namespace EnergyService.Domain.Events;

public sealed record ProcessCreatedEvent(
    int ProcessId,
    string Description,
    string UnitCode) : INotification;
