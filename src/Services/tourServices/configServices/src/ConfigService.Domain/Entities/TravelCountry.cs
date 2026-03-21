using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class TravelCountry : AggregateRoot<string>
{
    public string CountryName { get; private set; } = string.Empty;
    public string AirCode { get; private set; } = string.Empty;
    public string LastModifiedBy { get; private set; } = string.Empty;
    public DateTime LastModifiedOn { get; private set; }
    public string? GhAvailable { get; private set; }
    public string? GhRate { get; private set; }
    public string? NmsGhRate { get; private set; }

    private readonly List<TravelCountryModeMap> _modeMaps = [];
    public IReadOnlyCollection<TravelCountryModeMap> ModeMaps => _modeMaps.AsReadOnly();

    private readonly List<TravelCountrySectorMap> _sectorMaps = [];
    public IReadOnlyCollection<TravelCountrySectorMap> SectorMaps => _sectorMaps.AsReadOnly();

    private readonly List<TravelCountryCurrencyMap> _currencyMaps = [];
    public IReadOnlyCollection<TravelCountryCurrencyMap> CurrencyMaps => _currencyMaps.AsReadOnly();

    private TravelCountry() { }

    public static TravelCountry Create(string id, string name, string airCode, string modifiedBy)
    {
        var entity = new TravelCountry
        {
            Id = id, CountryName = name, AirCode = airCode,
            LastModifiedBy = modifiedBy, LastModifiedOn = DateTime.UtcNow
        };
        entity.AddDomainEvent(new Events.CountryCreatedEvent(id, name));
        return entity;
    }

    public void Update(string name, string airCode, string modifiedBy,
        string? ghAvailable = null, string? ghRate = null, string? nmsGhRate = null)
    {
        CountryName = name;
        AirCode = airCode;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
        GhAvailable = ghAvailable;
        GhRate = ghRate;
        NmsGhRate = nmsGhRate;
    }

    public void AddModeMap(TravelCountryModeMap map) => _modeMaps.Add(map);
    public void AddSectorMap(TravelCountrySectorMap map) => _sectorMaps.Add(map);
    public void AddCurrencyMap(TravelCountryCurrencyMap map) => _currencyMaps.Add(map);
}
