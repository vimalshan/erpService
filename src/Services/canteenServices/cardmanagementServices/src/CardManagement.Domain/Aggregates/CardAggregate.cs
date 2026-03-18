using CardManagement.Domain.Common;
using CardManagement.Domain.Entities;
using CardManagement.Domain.Events;
using CardManagement.Domain.ValueObjects;

namespace CardManagement.Domain.Aggregates;

/// <summary>
/// Card aggregate root — owns a GuestCardMaster and its associated CanteenCardMaps.
/// </summary>
public sealed class CardAggregate : AggregateRoot
{
    private readonly List<CanteenCardMap> _cardMaps = new();

    public GuestCardMaster GuestCard { get; private set; } = default!;
    public IReadOnlyList<CanteenCardMap> CardMaps => _cardMaps.AsReadOnly();

    private CardAggregate() { }

    public static CardAggregate Create(
        long canteenUnit, long cardSequence, string cardNumber, string cardName,
        string? cardType, string? reportingUnit, decimal? reportingDepartment,
        DateTime effectiveDate, decimal enteredByUser)
    {
        var agg = new CardAggregate { Id = canteenUnit };
        agg.GuestCard = GuestCardMaster.Create(canteenUnit, cardSequence, cardNumber, cardName,
            cardType, reportingUnit, reportingDepartment, effectiveDate, enteredByUser);
        return agg;
    }

    public void AssignCardMap(decimal sysId, long canteenUnit, string cardNumber, DateTime effectiveDate, decimal updatedByUser)
    {
        var map = CanteenCardMap.Create(sysId, canteenUnit, cardNumber, effectiveDate, updatedByUser);
        _cardMaps.Add(map);
    }

    public void CloseCard(decimal updatedByUser)
    {
        GuestCard.Close(updatedByUser);
        foreach (var map in _cardMaps.Where(m => m.ClosingDate == null))
            map.Close(updatedByUser);
    }
}
