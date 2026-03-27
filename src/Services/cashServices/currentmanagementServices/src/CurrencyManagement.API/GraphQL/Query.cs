using MediatR;
using CurrencyManagement.Application.DTOs;
using CurrencyManagement.Application.Currencies.Queries.GetAllCurrencies;
using CurrencyManagement.Application.Currencies.Queries.GetCurrencyById;
using CurrencyManagement.Application.ExchangeRates.Queries.GetExchangeRate;
using CurrencyManagement.Application.ExchangeRates.Queries.ConvertAmount;
using CurrencyManagement.Application.OrganizationCurrencies.Queries.GetOrganizationCurrencies;

namespace CurrencyManagement.API.GraphQL;

public class Query
{
    public async Task<IList<CurrencyDto>> GetCurrencies([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllCurrenciesQuery(), ct);

    public async Task<CurrencyDto?> GetCurrency([Service] IMediator mediator, long id, CancellationToken ct)
        => await mediator.Send(new GetCurrencyByIdQuery(id), ct);

    public async Task<ExchangeRateDto> GetExchangeRate([Service] IMediator mediator,
        long fromCurrencyId, long toCurrencyId, long financialYear, long month, CancellationToken ct)
        => await mediator.Send(new GetExchangeRateQuery(fromCurrencyId, toCurrencyId, financialYear, month), ct);

    public async Task<ConvertedAmountDto> GetConvertAmount([Service] IMediator mediator,
        long fromCurrencyId, long toCurrencyId, decimal amount, long financialYear, long month, CancellationToken ct)
        => await mediator.Send(new ConvertAmountQuery(fromCurrencyId, toCurrencyId, amount, financialYear, month), ct);

    public async Task<IList<OrganizationCurrencyDto>> GetOrganizationCurrencies([Service] IMediator mediator,
        long organizationId, CancellationToken ct)
        => await mediator.Send(new GetOrganizationCurrenciesQuery(organizationId), ct);
}
