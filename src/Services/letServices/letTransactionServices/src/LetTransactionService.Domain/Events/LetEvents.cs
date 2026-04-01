using LetTransactionService.Domain.Common;

namespace LetTransactionService.Domain.Events;

public sealed record LetRequestCreatedEvent(long RequestNumber, string EmployeeUserId) : DomainEvent;
public sealed record LetRequestSubAddedEvent(long RequestNumber, int SerialNumber) : DomainEvent;
public sealed record LetRequestSubUpdatedEvent(long RequestNumber, int SerialNumber) : DomainEvent;
