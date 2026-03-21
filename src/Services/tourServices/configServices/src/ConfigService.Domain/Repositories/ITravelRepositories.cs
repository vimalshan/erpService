using ConfigService.Domain.Common;
using ConfigService.Domain.Entities;

namespace ConfigService.Domain.Repositories;

public interface ITravelCityRepository : IRepository<TravelCity, string>
{
    Task<TravelCity?> GetWithMapsAsync(string id, CancellationToken ct = default);
    Task<IReadOnlyList<TravelCity>> GetByCountryAsync(string countryId, CancellationToken ct = default);
}

public interface ITravelCountryRepository : IRepository<TravelCountry, string>
{
    Task<TravelCountry?> GetWithMapsAsync(string id, CancellationToken ct = default);
}

public interface ITravelClassRepository : IRepository<TravelClass, string>
{
    Task<IReadOnlyList<TravelClass>> GetByModeAsync(string modeId, CancellationToken ct = default);
}

public interface ITravelContactRepository : IRepository<TravelContact, string> { }

public interface ITravelBusCitySectorMapRepository : IRepository<TravelBusCitySectorMap, string> { }

public interface ITravelBuExcludeRepository : IRepository<TravelBuExclude, string> { }
