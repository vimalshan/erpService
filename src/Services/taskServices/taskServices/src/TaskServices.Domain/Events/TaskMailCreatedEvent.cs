using TaskServices.Domain.Common;

namespace TaskServices.Domain.Events;

public record TaskMailCreatedEvent(decimal MailId, decimal SystemUserId) : IDomainEvent;
