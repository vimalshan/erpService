using MediatR;
using CurrencyManagement.Application.DTOs;
using CurrencyManagement.Application.Currencies.Commands.CreateCurrency;
using CurrencyManagement.Application.Currencies.Commands.UpdateCurrency;
using CurrencyManagement.Application.Currencies.Commands.DeleteCurrency;
using CurrencyManagement.Application.ExchangeRates.Commands.SetExchangeRate;
using CurrencyManagement.Application.OrganizationCurrencies.Commands.MapOrganizationCurrency;

namespace CurrencyManagement.API.GraphQL;

public class Mutation
{
    public async Task<CurrencyDto> CreateCurrency([Service] IMediator mediator, CreateCurrencyInput input, CancellationToken ct)
        => await mediator.Send(new CreateCurrencyCommand(input.CurrencyId, input.Name, input.Symbol, input.ModifiedBy), ct);

    public async Task<CurrencyDto> UpdateCurrency([Service] IMediator mediator, UpdateCurrencyInput input, CancellationToken ct)
        => await mediator.Send(new UpdateCurrencyCommand(input.CurrencyId, input.Name, input.Symbol, input.ModifiedBy), ct);

    public async Task<bool> DeleteCurrency([Service] IMediator mediator, long currencyId, CancellationToken ct)
    {
        await mediator.Send(new DeleteCurrencyCommand(currencyId), ct);
        return true;
    }

    public async Task<ExchangeRateDto> SetExchangeRate([Service] IMediator mediator, SetExchangeRateInput input, CancellationToken ct)
        => await mediator.Send(new SetExchangeRateCommand(
            input.RateId, input.FinancialYear, input.Month,
            input.FromCurrencyId, input.ToCurrencyId, input.Rate, input.ModifiedBy), ct);

    public async Task<OrganizationCurrencyDto> MapOrganizationCurrency([Service] IMediator mediator, MapOrganizationCurrencyInput input, CancellationToken ct)
        => await mediator.Send(new MapOrganizationCurrencyCommand(input.OrganizationId, input.CurrencyId, input.ModifiedBy), ct);
}

// Input types for GraphQL mutations
public record CreateCurrencyInput(long CurrencyId, string Name, string Symbol, long ModifiedBy);
public record UpdateCurrencyInput(long CurrencyId, string Name, string Symbol, long ModifiedBy);
public record SetExchangeRateInput(long RateId, long FinancialYear, long Month, long FromCurrencyId, long ToCurrencyId, decimal Rate, long ModifiedBy);
public record MapOrganizationCurrencyInput(long OrganizationId, long CurrencyId, long ModifiedBy);
