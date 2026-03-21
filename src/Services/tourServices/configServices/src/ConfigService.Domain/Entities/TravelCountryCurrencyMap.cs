using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class TravelCountryCurrencyMap : BaseEntity<string>
{
    public string CurrencyId { get; private set; } = string.Empty;
    public string CountryId { get; private set; } = string.Empty;
    public string LastModifiedBy { get; private set; } = string.Empty;
    public DateTime LastModifiedOn { get; private set; }

    private TravelCountryCurrencyMap() { }

    public static TravelCountryCurrencyMap Create(string mapId, string currencyId, string countryId, string modifiedBy)
    {
        return new TravelCountryCurrencyMap
        {
            Id = mapId, CurrencyId = currencyId, CountryId = countryId,
            LastModifiedBy = modifiedBy, LastModifiedOn = DateTime.UtcNow
        };
    }
}
