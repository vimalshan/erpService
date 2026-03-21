namespace ExpenseService.Application.DTOs;

public record CurrencyDto
{
    public long RequestNumber { get; init; }
    public int SerialNumber { get; init; }
    public string? CurrencyCode { get; init; }
    public long? CashAmount { get; init; }
    public long? TravellerChequeAmount { get; init; }
    public string? DenominationFlag { get; init; }
    public string? DenominationText { get; init; }
}
