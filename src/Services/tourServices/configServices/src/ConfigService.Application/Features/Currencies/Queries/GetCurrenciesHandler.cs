using ConfigService.Application.DTOs;
using ConfigService.Domain.Entities;
using ConfigService.Domain.Repositories;
using MediatR;

namespace ConfigService.Application.Features.Currencies.Queries;

public class GetAllCurrenciesHandler(ICurrencyRepository repo) : IRequestHandler<GetAllCurrenciesQuery, IReadOnlyList<CurrencyDto>>
{
    public async Task<IReadOnlyList<CurrencyDto>> Handle(GetAllCurrenciesQuery request, CancellationToken ct)
    {
        var currencies = await repo.GetAllAsync(ct);
        return currencies.Select(c => new CurrencyDto(c.Id, c.CurrencyCode, c.CurrencyName, c.CurrencySymbol)).ToList();
    }
}

public class GetCurrencyByIdHandler(ICurrencyRepository repo) : IRequestHandler<GetCurrencyByIdQuery, CurrencyDto?>
{
    public async Task<CurrencyDto?> Handle(GetCurrencyByIdQuery request, CancellationToken ct)
    {
        var c = await repo.GetByIdAsync(request.Id, ct);
        return c is null ? null : new CurrencyDto(c.Id, c.CurrencyCode, c.CurrencyName, c.CurrencySymbol);
    }
}
