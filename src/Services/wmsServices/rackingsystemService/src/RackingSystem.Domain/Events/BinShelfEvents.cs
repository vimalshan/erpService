using MediatR;

namespace RackingSystem.Domain.Events;

public record BinStatusChangedEvent(int BinId, string PreviousStatus, string NewStatus) : INotification;
public record BinCreatedEvent(int BinId, string Code) : INotification;
public record BinDeactivatedEvent(int BinId) : INotification;

public record ShelfCreatedEvent(int ShelfId, int RackId) : INotification;
public record ShelfUpdatedEvent(int ShelfId) : INotification;
