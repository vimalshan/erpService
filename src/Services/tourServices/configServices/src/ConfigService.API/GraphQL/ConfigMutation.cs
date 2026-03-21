using ConfigService.Application.DTOs;
using ConfigService.Application.Features.Currencies.Commands;
using ConfigService.Application.Features.Travel.Commands;
using ConfigService.Application.Features.Vendors.Commands;
using MediatR;

namespace ConfigService.API.GraphQL;

public class ConfigMutation
{
    [GraphQLDescription("Create a new currency")]
    public async Task<CurrencyDto> CreateCurrency([Service] IMediator mediator, CreateCurrencyCommand input, CancellationToken ct) =>
        await mediator.Send(input, ct);

    [GraphQLDescription("Update a currency")]
    public async Task<CurrencyDto> UpdateCurrency([Service] IMediator mediator, UpdateCurrencyCommand input, CancellationToken ct) =>
        await mediator.Send(input, ct);

    [GraphQLDescription("Create a new country")]
    public async Task<TravelCountryDto> CreateCountry([Service] IMediator mediator, CreateCountryCommand input, CancellationToken ct) =>
        await mediator.Send(input, ct);

    [GraphQLDescription("Create a new city")]
    public async Task<TravelCityDto> CreateCity([Service] IMediator mediator, CreateCityCommand input, CancellationToken ct) =>
        await mediator.Send(input, ct);

    [GraphQLDescription("Create a new vendor")]
    public async Task<VendorDto> CreateVendor([Service] IMediator mediator, CreateVendorCommand input, CancellationToken ct) =>
        await mediator.Send(input, ct);

    [GraphQLDescription("Update a vendor")]
    public async Task<VendorDto> UpdateVendor([Service] IMediator mediator, UpdateVendorCommand input, CancellationToken ct) =>
        await mediator.Send(input, ct);
}
