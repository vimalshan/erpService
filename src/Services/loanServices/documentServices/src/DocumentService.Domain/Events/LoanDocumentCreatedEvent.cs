using DocumentService.Domain.Common;

namespace DocumentService.Domain.Events;

public record LoanDocumentCreatedEvent(long DocumentId, long LoanId, long TypeId) : DomainEvent;
