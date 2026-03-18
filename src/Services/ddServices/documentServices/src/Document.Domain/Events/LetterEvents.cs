using Document.Domain.Common;
using Document.Domain.Entities;

namespace Document.Domain.Events;

public sealed class LetterGeneratedEvent : DomainEvent
{
    public GeneratedLetter Letter { get; }
    public LetterGeneratedEvent(GeneratedLetter letter) => Letter = letter;
}

public sealed class LetterOpenedEvent : DomainEvent
{
    public decimal EmployeeSysId { get; }
    public string LetterType { get; }
    public string IpAddress { get; }

    public LetterOpenedEvent(decimal employeeSysId, string letterType, string ipAddress)
    {
        EmployeeSysId = employeeSysId;
        LetterType = letterType;
        IpAddress = ipAddress;
    }
}
