using ConfigService.Application.DTOs;
using MediatR;

namespace ConfigService.Application.Features.Currencies.Commands;

public record CreateCurrencyCommand(string CurrencyCode, string? CurrencyName, string? CurrencySymbol) : IRequest<CurrencyDto>;

public record UpdateCurrencyCommand(long CurrencyId, string CurrencyCode, string? CurrencyName, string? CurrencySymbol) : IRequest<CurrencyDto>;

public record DeleteCurrencyCommand(long CurrencyId) : IRequest<bool>;
