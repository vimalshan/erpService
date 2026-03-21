using ConfigService.Application.DTOs;
using MediatR;

namespace ConfigService.Application.Features.Currencies.Queries;

public record GetAllCurrenciesQuery : IRequest<IReadOnlyList<CurrencyDto>>;
public record GetCurrencyByIdQuery(long Id) : IRequest<CurrencyDto?>;
