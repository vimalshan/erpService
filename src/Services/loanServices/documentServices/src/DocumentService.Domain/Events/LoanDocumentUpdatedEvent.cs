using DocumentService.Domain.Common;

namespace DocumentService.Domain.Events;

public record LoanDocumentUpdatedEvent(long DocumentId, long LoanId, long NewTypeId) : DomainEvent;
