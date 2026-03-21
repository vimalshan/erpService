using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class TravelCountryModeMap : BaseEntity<string>
{
    public string ModeId { get; private set; } = string.Empty;
    public string CountryId { get; private set; } = string.Empty;
    public string LastModifiedBy { get; private set; } = string.Empty;
    public DateTime LastModifiedOn { get; private set; }

    private TravelCountryModeMap() { }

    public static TravelCountryModeMap Create(string mapId, string modeId, string countryId, string modifiedBy)
    {
        return new TravelCountryModeMap
        {
            Id = mapId, ModeId = modeId, CountryId = countryId,
            LastModifiedBy = modifiedBy, LastModifiedOn = DateTime.UtcNow
        };
    }
}
