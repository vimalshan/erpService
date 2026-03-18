using CardManagement.Domain.Common;
using CardManagement.Domain.Events;
using CN = CardManagement.Domain.ValueObjects.CardNumber;

namespace CardManagement.Domain.Entities;

public class CanteenCardMap : BaseEntity
{
    public decimal SysId { get; private set; }
    public long CanteenUnit { get; private set; }
    public string CardNumber { get; private set; } = default!;
    public DateTime? EffectiveDate { get; private set; }
    public DateTime? ClosingDate { get; private set; }
    public decimal? UpdatedByUser { get; private set; }
    public DateTime? UpdatedDate { get; private set; }

    private CanteenCardMap() { }

    public static CanteenCardMap Create(decimal sysId, long canteenUnit, string cardNumber, DateTime effectiveDate, decimal updatedByUser)
    {
        var card = new CanteenCardMap
        {
            SysId = sysId,
            CanteenUnit = canteenUnit,
            CardNumber = CN.Create(cardNumber).Value,
            EffectiveDate = effectiveDate,
            UpdatedByUser = updatedByUser,
            UpdatedDate = DateTime.UtcNow
        };
        card.AddDomainEvent(new CardMapCreatedEvent(sysId, canteenUnit, cardNumber));
        return card;
    }

    public void Close(decimal updatedByUser)
    {
        ClosingDate = DateTime.UtcNow;
        UpdatedByUser = updatedByUser;
        UpdatedDate = DateTime.UtcNow;
        AddDomainEvent(new CardMapClosedEvent(SysId, CanteenUnit, CardNumber));
    }
}
