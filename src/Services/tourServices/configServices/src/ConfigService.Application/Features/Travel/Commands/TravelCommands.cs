using ConfigService.Application.DTOs;
using MediatR;

namespace ConfigService.Application.Features.Travel.Commands;

public record CreateCountryCommand(string CountryId, string CountryName, string AirCode, string ModifiedBy) : IRequest<TravelCountryDto>;
public record UpdateCountryCommand(string CountryId, string CountryName, string AirCode, string ModifiedBy) : IRequest<TravelCountryDto>;
public record DeleteCountryCommand(string CountryId) : IRequest<bool>;

public record CreateCityCommand(string CityId, string CountryId, string CityName, string CityCode, string ModifiedBy) : IRequest<TravelCityDto>;
public record UpdateCityCommand(string CityId, string CountryId, string CityName, string CityCode, string ModifiedBy) : IRequest<TravelCityDto>;
public record DeleteCityCommand(string CityId) : IRequest<bool>;
