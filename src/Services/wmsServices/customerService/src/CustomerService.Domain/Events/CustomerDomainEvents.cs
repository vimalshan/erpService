using MediatR;

namespace CustomerService.Domain.Events;

public sealed record CustomerCreatedEvent(Domain.Entities.Customer Customer) : INotification;
public sealed record CustomerUpdatedEvent(Domain.Entities.Customer Customer) : INotification;
public sealed record CustomerActivatedEvent(int CustomerId) : INotification;
public sealed record CustomerDeactivatedEvent(int CustomerId) : INotification;
public sealed record CustomerDeletedEvent(int CustomerId) : INotification;
