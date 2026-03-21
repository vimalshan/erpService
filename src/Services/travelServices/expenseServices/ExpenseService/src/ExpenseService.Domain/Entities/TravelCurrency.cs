using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Entities;

public class TravelCurrency : BaseEntity
{
    public long RequestNumber { get; set; }
    public int SerialNumber { get; set; }
    public string? CurrencyCode { get; set; }
    public long? CashAmount { get; set; }
    public long? TravellerChequeAmount { get; set; }
    public string? DenominationFlag { get; set; }
    public string? DenominationText { get; set; }
}
