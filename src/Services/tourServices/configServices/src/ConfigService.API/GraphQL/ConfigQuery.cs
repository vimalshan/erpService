using ConfigService.Application.DTOs;
using ConfigService.Application.Features.Currencies.Queries;
using ConfigService.Application.Features.Travel.Queries;
using ConfigService.Application.Features.Vendors.Queries;
using MediatR;

namespace ConfigService.API.GraphQL;

public class ConfigQuery
{
    [GraphQLDescription("Get all currencies")]
    public async Task<IReadOnlyList<CurrencyDto>> GetCurrencies([Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetAllCurrenciesQuery(), ct);

    [GraphQLDescription("Get currency by ID")]
    public async Task<CurrencyDto?> GetCurrencyById([Service] IMediator mediator, long id, CancellationToken ct) =>
        await mediator.Send(new GetCurrencyByIdQuery(id), ct);

    [GraphQLDescription("Get all countries")]
    public async Task<IReadOnlyList<TravelCountryDto>> GetCountries([Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetAllCountriesQuery(), ct);

    [GraphQLDescription("Get country by ID")]
    public async Task<TravelCountryDto?> GetCountryById([Service] IMediator mediator, string id, CancellationToken ct) =>
        await mediator.Send(new GetCountryByIdQuery(id), ct);

    [GraphQLDescription("Get all cities")]
    public async Task<IReadOnlyList<TravelCityDto>> GetCities([Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetAllCitiesQuery(), ct);

    [GraphQLDescription("Get cities by country")]
    public async Task<IReadOnlyList<TravelCityDto>> GetCitiesByCountry([Service] IMediator mediator, string countryId, CancellationToken ct) =>
        await mediator.Send(new GetCitiesByCountryQuery(countryId), ct);

    [GraphQLDescription("Get all vendors")]
    public async Task<IReadOnlyList<VendorDto>> GetVendors([Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetAllVendorsQuery(), ct);

    [GraphQLDescription("Get vendor by ID")]
    public async Task<VendorDto?> GetVendorById([Service] IMediator mediator, string id, CancellationToken ct) =>
        await mediator.Send(new GetVendorByIdQuery(id), ct);

    [GraphQLDescription("Get active vendors")]
    public async Task<IReadOnlyList<VendorDto>> GetActiveVendors([Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetActiveVendorsQuery(), ct);

    [GraphQLDescription("Get all travel classes")]
    public async Task<IReadOnlyList<TravelClassDto>> GetTravelClasses([Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetAllTravelClassesQuery(), ct);

    [GraphQLDescription("Get all travel contacts")]
    public async Task<IReadOnlyList<TravelContactDto>> GetTravelContacts([Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetAllTravelContactsQuery(), ct);
}
