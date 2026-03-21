using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class TravelCity : AggregateRoot<string>
{
    public string CountryId { get; private set; } = string.Empty;
    public string CityName { get; private set; } = string.Empty;
    public string CityCode { get; private set; } = string.Empty;
    public string LastModifiedBy { get; private set; } = string.Empty;
    public DateTime LastModifiedOn { get; private set; }

    private readonly List<TravelCityModeMap> _modeMaps = [];
    public IReadOnlyCollection<TravelCityModeMap> ModeMaps => _modeMaps.AsReadOnly();

    private readonly List<TravelCitySectorMap> _sectorMaps = [];
    public IReadOnlyCollection<TravelCitySectorMap> SectorMaps => _sectorMaps.AsReadOnly();

    private TravelCity() { }

    public static TravelCity Create(string id, string countryId, string name, string code, string modifiedBy)
    {
        var entity = new TravelCity
        {
            Id = id, CountryId = countryId, CityName = name, CityCode = code,
            LastModifiedBy = modifiedBy, LastModifiedOn = DateTime.UtcNow
        };
        entity.AddDomainEvent(new Events.CityCreatedEvent(id, name));
        return entity;
    }

    public void Update(string countryId, string name, string code, string modifiedBy)
    {
        CountryId = countryId;
        CityName = name;
        CityCode = code;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void AddModeMap(TravelCityModeMap map) => _modeMaps.Add(map);
    public void AddSectorMap(TravelCitySectorMap map) => _sectorMaps.Add(map);
}
