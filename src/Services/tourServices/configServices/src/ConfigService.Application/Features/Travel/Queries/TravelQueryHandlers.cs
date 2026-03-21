using ConfigService.Application.DTOs;
using ConfigService.Domain.Repositories;
using MediatR;

namespace ConfigService.Application.Features.Travel.Queries;

public class GetAllCountriesHandler(ITravelCountryRepository repo) : IRequestHandler<GetAllCountriesQuery, IReadOnlyList<TravelCountryDto>>
{
    public async Task<IReadOnlyList<TravelCountryDto>> Handle(GetAllCountriesQuery request, CancellationToken ct)
    {
        var items = await repo.GetAllAsync(ct);
        return items.Select(c => new TravelCountryDto(c.Id, c.CountryName, c.AirCode,
            c.LastModifiedBy, c.LastModifiedOn, c.GhAvailable, c.GhRate, c.NmsGhRate)).ToList();
    }
}

public class GetCountryByIdHandler(ITravelCountryRepository repo) : IRequestHandler<GetCountryByIdQuery, TravelCountryDto?>
{
    public async Task<TravelCountryDto?> Handle(GetCountryByIdQuery request, CancellationToken ct)
    {
        var c = await repo.GetByIdAsync(request.Id, ct);
        return c is null ? null : new TravelCountryDto(c.Id, c.CountryName, c.AirCode,
            c.LastModifiedBy, c.LastModifiedOn, c.GhAvailable, c.GhRate, c.NmsGhRate);
    }
}

public class GetAllCitiesHandler(ITravelCityRepository repo) : IRequestHandler<GetAllCitiesQuery, IReadOnlyList<TravelCityDto>>
{
    public async Task<IReadOnlyList<TravelCityDto>> Handle(GetAllCitiesQuery request, CancellationToken ct)
    {
        var items = await repo.GetAllAsync(ct);
        return items.Select(c => new TravelCityDto(c.Id, c.CountryId, c.CityName, c.CityCode,
            c.LastModifiedBy, c.LastModifiedOn)).ToList();
    }
}

public class GetCityByIdHandler(ITravelCityRepository repo) : IRequestHandler<GetCityByIdQuery, TravelCityDto?>
{
    public async Task<TravelCityDto?> Handle(GetCityByIdQuery request, CancellationToken ct)
    {
        var c = await repo.GetByIdAsync(request.Id, ct);
        return c is null ? null : new TravelCityDto(c.Id, c.CountryId, c.CityName, c.CityCode,
            c.LastModifiedBy, c.LastModifiedOn);
    }
}

public class GetCitiesByCountryHandler(ITravelCityRepository repo) : IRequestHandler<GetCitiesByCountryQuery, IReadOnlyList<TravelCityDto>>
{
    public async Task<IReadOnlyList<TravelCityDto>> Handle(GetCitiesByCountryQuery request, CancellationToken ct)
    {
        var items = await repo.GetByCountryAsync(request.CountryId, ct);
        return items.Select(c => new TravelCityDto(c.Id, c.CountryId, c.CityName, c.CityCode,
            c.LastModifiedBy, c.LastModifiedOn)).ToList();
    }
}

public class GetAllTravelClassesHandler(ITravelClassRepository repo) : IRequestHandler<GetAllTravelClassesQuery, IReadOnlyList<TravelClassDto>>
{
    public async Task<IReadOnlyList<TravelClassDto>> Handle(GetAllTravelClassesQuery request, CancellationToken ct)
    {
        var items = await repo.GetAllAsync(ct);
        return items.Select(c => new TravelClassDto(c.Id, c.ModeId, c.ClassName, c.ClassOrder)).ToList();
    }
}

public class GetAllTravelContactsHandler(ITravelContactRepository repo) : IRequestHandler<GetAllTravelContactsQuery, IReadOnlyList<TravelContactDto>>
{
    public async Task<IReadOnlyList<TravelContactDto>> Handle(GetAllTravelContactsQuery request, CancellationToken ct)
    {
        var items = await repo.GetAllAsync(ct);
        return items.Select(c => new TravelContactDto(c.Id, c.ContactType, c.AdminId,
            c.AdminName, c.EmployeeSysId, c.PhoneNos, c.EmailId)).ToList();
    }
}
