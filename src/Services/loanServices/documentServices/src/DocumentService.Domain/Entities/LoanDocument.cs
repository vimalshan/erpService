using DocumentService.Domain.Common;
using DocumentService.Domain.Events;

namespace DocumentService.Domain.Entities;

public class LoanDocument : AggregateRoot
{
    public long Id { get; private set; }
    public long LoanId { get; private set; }
    public long TypeId { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    private LoanDocument() { } // Required by EF Core

    public static LoanDocument Create(long id, long loanId, long typeId, long modifiedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(loanId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(typeId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(modifiedBy);

        var document = new LoanDocument
        {
            Id = id,
            LoanId = loanId,
            TypeId = typeId,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
        document.RaiseDomainEvent(new LoanDocumentCreatedEvent(document.Id, document.LoanId, document.TypeId));
        return document;
    }

    public void Update(long typeId, long modifiedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(typeId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(modifiedBy);

        TypeId = typeId;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
        RaiseDomainEvent(new LoanDocumentUpdatedEvent(Id, LoanId, TypeId));
    }

    public void MarkDeleted()
    {
        RaiseDomainEvent(new LoanDocumentDeletedEvent(Id, LoanId));
    }
}
