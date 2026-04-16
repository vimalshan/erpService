using MediatR;

namespace NotificationService.Domain.Events;

public interface IDomainEvent : INotification { }

public record NotificationCreatedEvent(int NotificationId, string Title, string Priority, int CategoryId) : IDomainEvent;
public record NotificationReadEvent(int NotificationId, int UserId) : IDomainEvent;
public record NotificationArchivedEvent(int NotificationId) : IDomainEvent;
