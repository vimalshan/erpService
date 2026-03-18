using Document.Domain.Common;
using Document.Domain.Entities;

namespace Document.Domain.Events;

public sealed class SignatoryCreatedEvent : DomainEvent
{
    public Signatory Signatory { get; }
    public SignatoryCreatedEvent(Signatory signatory) => Signatory = signatory;
}

public sealed class SignatoryUpdatedEvent : DomainEvent
{
    public Signatory Signatory { get; }
    public SignatoryUpdatedEvent(Signatory signatory) => Signatory = signatory;
}
