using ConfigService.Application.DTOs;
using ConfigService.Domain.Common;
using ConfigService.Domain.Entities;
using ConfigService.Domain.Repositories;
using MediatR;

namespace ConfigService.Application.Features.Travel.Commands;

public class CreateCountryHandler(ITravelCountryRepository repo, IUnitOfWork uow) : IRequestHandler<CreateCountryCommand, TravelCountryDto>
{
    public async Task<TravelCountryDto> Handle(CreateCountryCommand r, CancellationToken ct)
    {
        var country = TravelCountry.Create(r.CountryId, r.CountryName, r.AirCode, r.ModifiedBy);
        await repo.AddAsync(country, ct);
        await uow.SaveChangesAsync(ct);
        return new TravelCountryDto(country.Id, country.CountryName, country.AirCode,
            country.LastModifiedBy, country.LastModifiedOn, country.GhAvailable, country.GhRate, country.NmsGhRate);
    }
}

public class UpdateCountryHandler(ITravelCountryRepository repo, IUnitOfWork uow) : IRequestHandler<UpdateCountryCommand, TravelCountryDto>
{
    public async Task<TravelCountryDto> Handle(UpdateCountryCommand r, CancellationToken ct)
    {
        var country = await repo.GetByIdAsync(r.CountryId, ct) ?? throw new KeyNotFoundException($"Country {r.CountryId} not found.");
        country.Update(r.CountryName, r.AirCode, r.ModifiedBy);
        await repo.UpdateAsync(country, ct);
        await uow.SaveChangesAsync(ct);
        return new TravelCountryDto(country.Id, country.CountryName, country.AirCode,
            country.LastModifiedBy, country.LastModifiedOn, country.GhAvailable, country.GhRate, country.NmsGhRate);
    }
}

public class DeleteCountryHandler(ITravelCountryRepository repo, IUnitOfWork uow) : IRequestHandler<DeleteCountryCommand, bool>
{
    public async Task<bool> Handle(DeleteCountryCommand r, CancellationToken ct)
    {
        var country = await repo.GetByIdAsync(r.CountryId, ct);
        if (country is null) return false;
        await repo.DeleteAsync(country, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class CreateCityHandler(ITravelCityRepository repo, IUnitOfWork uow) : IRequestHandler<CreateCityCommand, TravelCityDto>
{
    public async Task<TravelCityDto> Handle(CreateCityCommand r, CancellationToken ct)
    {
        var city = TravelCity.Create(r.CityId, r.CountryId, r.CityName, r.CityCode, r.ModifiedBy);
        await repo.AddAsync(city, ct);
        await uow.SaveChangesAsync(ct);
        return new TravelCityDto(city.Id, city.CountryId, city.CityName, city.CityCode, city.LastModifiedBy, city.LastModifiedOn);
    }
}

public class UpdateCityHandler(ITravelCityRepository repo, IUnitOfWork uow) : IRequestHandler<UpdateCityCommand, TravelCityDto>
{
    public async Task<TravelCityDto> Handle(UpdateCityCommand r, CancellationToken ct)
    {
        var city = await repo.GetByIdAsync(r.CityId, ct) ?? throw new KeyNotFoundException($"City {r.CityId} not found.");
        city.Update(r.CountryId, r.CityName, r.CityCode, r.ModifiedBy);
        await repo.UpdateAsync(city, ct);
        await uow.SaveChangesAsync(ct);
        return new TravelCityDto(city.Id, city.CountryId, city.CityName, city.CityCode, city.LastModifiedBy, city.LastModifiedOn);
    }
}

public class DeleteCityHandler(ITravelCityRepository repo, IUnitOfWork uow) : IRequestHandler<DeleteCityCommand, bool>
{
    public async Task<bool> Handle(DeleteCityCommand r, CancellationToken ct)
    {
        var city = await repo.GetByIdAsync(r.CityId, ct);
        if (city is null) return false;
        await repo.DeleteAsync(city, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
