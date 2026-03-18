using CardManagement.Domain.Common;
using CardManagement.Domain.Enums;
using CardManagement.Domain.ValueObjects;
using CardManagement.Domain.Events;

namespace CardManagement.Domain.Entities;

public class GuestCardMaster : AggregateRoot
{
    public long CanteenUnit { get; private set; }
    public long CardSequence { get; private set; }
    public string? CardNumber { get; private set; }
    public string? CardName { get; private set; }
    public string? ReportingUnit { get; private set; }
    public decimal? ReportingDepartment { get; private set; }
    public string? CardType { get; private set; }
    public decimal? EnteredByUser { get; private set; }
    public DateTime? EnteredOn { get; private set; }
    public DateTime? EffectiveDate { get; private set; }
    public DateTime? ClosingDate { get; private set; }

    public bool IsActive => ClosingDate == null || ClosingDate > DateTime.UtcNow;

    private GuestCardMaster() { }

    public static GuestCardMaster Create(
        long canteenUnit, long cardSequence, string cardNumber, string cardName,
        string? cardType, string? reportingUnit, decimal? reportingDepartment,
        DateTime effectiveDate, decimal enteredByUser)
    {
        var entity = new GuestCardMaster
        {
            Id = canteenUnit,
            CanteenUnit = canteenUnit,
            CardSequence = cardSequence,
            CardNumber = cardNumber,
            CardName = cardName,
            CardType = cardType,
            ReportingUnit = reportingUnit,
            ReportingDepartment = reportingDepartment,
            EffectiveDate = effectiveDate,
            EnteredByUser = enteredByUser,
            EnteredOn = DateTime.UtcNow
        };
        entity.AddDomainEvent(new GuestCardCreatedEvent(canteenUnit, cardSequence, cardNumber, cardName));
        return entity;
    }

    public void Update(string? cardName, string? cardType, string? reportingUnit, decimal? reportingDepartment)
    {
        CardName = cardName;
        CardType = cardType;
        ReportingUnit = reportingUnit;
        ReportingDepartment = reportingDepartment;
        AddDomainEvent(new GuestCardUpdatedEvent(CanteenUnit, CardSequence, CardNumber));
    }

    public void Close(decimal updatedByUser)
    {
        ClosingDate = DateTime.UtcNow;
        AddDomainEvent(new GuestCardClosedEvent(CanteenUnit, CardSequence, CardNumber));
    }
}
