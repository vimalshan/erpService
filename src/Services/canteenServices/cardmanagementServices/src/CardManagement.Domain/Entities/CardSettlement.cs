using CardManagement.Domain.Common;
using CardManagement.Domain.Events;
using CN = CardManagement.Domain.ValueObjects.CardNumber;

namespace CardManagement.Domain.Entities;

public class CardSettlement : BaseEntity
{
    public decimal SysId { get; private set; }
    public long CanteenUnit { get; private set; }
    public string? CardNumber { get; private set; }
    public DateTime? SettlementDate { get; private set; }
    public decimal? UpdatedByUser { get; private set; }
    public DateTime? UpdatedDate { get; private set; }

    private CardSettlement() { }

    public static CardSettlement Create(decimal sysId, long canteenUnit, string cardNumber, DateTime settlementDate, decimal updatedByUser)
    {
        var entity = new CardSettlement
        {
            SysId = sysId,
            CanteenUnit = canteenUnit,
            CardNumber = CN.Create(cardNumber).Value,
            SettlementDate = settlementDate,
            UpdatedByUser = updatedByUser,
            UpdatedDate = DateTime.UtcNow
        };
        entity.AddDomainEvent(new CardSettledEvent(sysId, canteenUnit, cardNumber, settlementDate));
        return entity;
    }
}
