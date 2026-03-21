using ExpenseService.Application.DTOs;
using MediatR;

namespace ExpenseService.Application.Commands;

public record CreateCurrencyRequestCommand : IRequest<CurrencyDto>
{
    public long RequestNumber { get; init; }
    public string? CurrencyCode { get; init; }
    public long? CashAmount { get; init; }
    public long? TravellerChequeAmount { get; init; }
    public string? DenominationFlag { get; init; }
    public string? DenominationText { get; init; }
}
