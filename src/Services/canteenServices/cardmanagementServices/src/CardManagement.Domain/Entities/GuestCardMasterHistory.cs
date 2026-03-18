namespace CardManagement.Domain.Entities;

public class GuestCardMasterHistory
{
    public long CanteenUnit { get; set; }
    public long CardSequence { get; set; }
    public string? CardNumber { get; set; }
    public string? CardName { get; set; }
    public string? ReportingUnit { get; set; }
    public decimal? ReportingDepartment { get; set; }
    public string? CardType { get; set; }
    public decimal? ModifiedByUser { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public static GuestCardMasterHistory FromGuestCard(GuestCardMaster card, decimal modifiedByUser)
        => new()
        {
            CanteenUnit = card.CanteenUnit,
            CardSequence = card.CardSequence,
            CardNumber = card.CardNumber,
            CardName = card.CardName,
            ReportingUnit = card.ReportingUnit,
            ReportingDepartment = card.ReportingDepartment,
            CardType = card.CardType,
            ModifiedByUser = modifiedByUser,
            ModifiedOn = DateTime.UtcNow
        };
}
