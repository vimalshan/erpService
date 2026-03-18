using CurrencyManagement.Application.Currencies.Commands.CreateCurrency;
using CurrencyManagement.Application.Currencies.Queries.GetAllCurrencies;
using CurrencyManagement.Application.ExchangeRates.Commands.SetExchangeRate;
using CurrencyManagement.Application.OrganizationCurrencies.Commands.MapOrganizationCurrency;
using CurrencyManagement.Application.OrganizationCurrencies.Queries.GetOrganizationCurrencies;
using MediatR;

namespace CurrencyManagement.API.Endpoints;

/// <summary>
/// Minimal API endpoints for Currency operations
/// </summary>
public static class CurrencyEndpoints
{
    public static void MapCurrencyEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/currencies")
            .WithName("Currencies")
            .WithOpenApi();

        group.MapGet("/", GetAllCurrencies)
            .WithName("GetAllCurrencies")
            .WithDescription("Get all currencies");

        group.MapPost("/", CreateCurrency)
            .WithName("CreateCurrency")
            .WithDescription("Create a new currency");
    }

    public static async Task<IResult> GetAllCurrencies(IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllCurrenciesQuery(), cancellationToken);
        return Results.Ok(result);
    }

    public static async Task<IResult> CreateCurrency(CreateCurrencyCommand command, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Results.CreatedAtRoute(nameof(CurrencyEndpoints.GetAllCurrencies), result);
    }
}

/// <summary>
/// Minimal API endpoints for Exchange Rate operations
/// </summary>
public static class ExchangeRateEndpoints
{
    public static void MapExchangeRateEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/exchange-rates")
            .WithName("ExchangeRates")
            .WithOpenApi();

        group.MapPost("/", SetExchangeRate)
            .WithName("SetExchangeRate")
            .WithDescription("Set or update an exchange rate");
    }

    public static async Task<IResult> SetExchangeRate(SetExchangeRateCommand command, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Results.Ok(result);
    }
}
