using ConfigService.Application.DTOs;
using MediatR;

namespace ConfigService.Application.Features.Travel.Queries;

public record GetAllCountriesQuery : IRequest<IReadOnlyList<TravelCountryDto>>;
public record GetCountryByIdQuery(string Id) : IRequest<TravelCountryDto?>;
public record GetAllCitiesQuery : IRequest<IReadOnlyList<TravelCityDto>>;
public record GetCityByIdQuery(string Id) : IRequest<TravelCityDto?>;
public record GetCitiesByCountryQuery(string CountryId) : IRequest<IReadOnlyList<TravelCityDto>>;
public record GetAllTravelClassesQuery : IRequest<IReadOnlyList<TravelClassDto>>;
public record GetAllTravelContactsQuery : IRequest<IReadOnlyList<TravelContactDto>>;
