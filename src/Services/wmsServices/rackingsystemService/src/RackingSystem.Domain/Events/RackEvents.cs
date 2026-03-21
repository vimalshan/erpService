using MediatR;
using RackingSystem.Domain.Entities;

namespace RackingSystem.Domain.Events;

public record RackCreatedEvent(Rack Rack) : INotification;
public record RackUpdatedEvent(Rack Rack) : INotification;
public record RackDeletedEvent(int RackId) : INotification;
