using TaskServices.Domain.Common;

namespace TaskServices.Domain.Events;

public record TaskMailReassignedEvent(decimal MailId, decimal OldSystemUserId, decimal NewSystemUserId) : IDomainEvent;
