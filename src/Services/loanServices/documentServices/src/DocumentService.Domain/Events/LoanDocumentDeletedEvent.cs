using DocumentService.Domain.Common;

namespace DocumentService.Domain.Events;

public record LoanDocumentDeletedEvent(long DocumentId, long LoanId) : DomainEvent;
