using MediatR;

namespace EnergyService.Domain.Events;

public sealed record ReadingRecordedEvent(
    int ProcessId,
    string UnitCode,
    long ReadingValue,
    long ActualUsage,
    DateTime RecordedAt) : INotification;
